<?php

namespace Users;

require_once(BASE_PATH . 'inc/database.inc.php');

//	define('SALT', 'Salt + Pepper');

function __generate_random_string($length = 10) {
	$characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
	$randomString = '';
	
	for ($i = 0; $i < $length; $i++)
		$randomString .= $characters[rand(0, strlen($characters) - 1)];
	
	return $randomString;
}

/*	function hash_key($username, $password) {
	return sha1($username . SALT . $password);
}	*/

function getUserIdentifier($username) {
	global $db_conn;
	
	$result = $db_conn->query("SELECT `id` FROM `users` WHERE `username` = %s", $username)->fetchAll();
	
	return $result[0]['id'];
}

function getUsername($userId) {
	global $db_conn;
	
	$result = $db_conn->query("SELECT `username` FROM `users` WHERE `id` = %s", $userId)->fetchAll();
	
	return $result[0]['username'];
}

function getLoggedInUserIdentifier($key) {
	global $db_conn;
	
	$result = $db_conn->query("SELECT `user` FROM `sessions` WHERE `key` = %s", $key)->fetchAll();
	
	if (!sizeof($result))
		return false;
	
	return $result[0]['user'];
}

function validateLoginDetails($username, $password) {
	global $db_conn;
	
	return ($db_conn->query("SELECT * FROM `users` WHERE `username` = %s AND `password` = %s", 
				$username, 
				/* hash_key($username, $password))->rowCount() */ $password
				/* == 1 */);
}

function login($username, $password, &$key) {
	global $db_conn;
	
	if (!validateLoginDetails($username, $password))
		return false;
	
	$key = __generate_random_string(128);
	
	while ($db_conn->query("SELECT `key` FROM `sessions` WHERE `key` = %s", $key)->rowCount())
		$key = __generate_random_string(128);
	
	$db_conn->query("INSERT INTO `sessions` (`user`, `key`) VALUES (%s, %s)", getUserIdentifier($username), $key);
	
	return true;
}

function validateSession($key) {
	global $db_conn;
	
	return ($db_conn->query("SELECT `key` FROM `sessions` WHERE `key` = %s", $key)->rowCount() == 1);
}

function createAccount($username, $password, $email, &$error) {
	global $db_conn;
	
	if (strlen($username) < 6) {
		$error = 'Username too short!';
		
		return false;
	}
	
	if (strlen($password) != 128) {
		$error = 'Passwords are required to be hashed using SHA-512.';
		
		return false;
	}
	
	if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
		$error = 'Invalid e-mail address!';
		
		return false;
	}
	
	if ($db_conn->query("SELECT * FROM `users` WHERE `username` = %s", $username)->rowCount()) {
		$error = 'Username already in use!';
		
		return false;
	}
	
	if ($db_conn->query("SELECT * FROM `users` WHERE `email` = %s", $email)->rowCount()) {
		$error = 'E-mail address already in use!';
		
		return false;
	}
	
	$db_conn->query("INSERT INTO `users` (`username`, `password`, `email`) VALUES (%s, %s, %s)", $username, /* hash_key($username, $password) */ $password, $email);
	
	return true;
}

function getPushDevices($userId) {
	global $db_conn;
	
	$returnArray = array();
	
	$result = $db_conn->query("SELECT * FROM `devices` WHERE `user` = %s", $userId)->fetchAll();
	
	foreach ($result as $d)
		$returnArray[] = array('type' => $d['type'], 'token' => $d['token']);
	
	return $returnArray;
}

function updatePushDevices($userId, $list) {
	global $db_conn;
	
	$db_conn->query("DELETE FROM `devices` WHERE `user` = %s", $userId);
	
	foreach ($list as $l)
		$db_conn->query("INSERT INTO `devices` (`user`, `type`, `token`) VALUES (%s, %s, %s)", $userId, $l['device'], $l['token']);
}

?>
