<?php

namespace Sync;

require_once(BASE_PATH . 'inc/database.inc.php');

function store($userId, $blob) {
	$query = $db_conn->query("SELECT `data` FROM `cloud` WHERE `user` = %s", $userId);
	
	if (!$query->numRows())
		$db_conn->query("INSERT INTO `cloud` (`data`, `user`) VALUES (%s, %s)", $blob, $userId);
	else
		$db_conn->query("UPDATE `cloud` SET `data` = %s WHERE `user` = %s", $blob, $userId);
}

function load($userId) {
	$query = $db_conn->query("SELECT `data` FROM `cloud` WHERE `user` = %s", $userId);
	
	if (!$query->numRows())
		return false;
	
	return $query->fetch()['data'];
}

function delete($userId) {
	$db_conn->query("DELETE FROM `cloud` WHERE `user` = %s", $userId);
}

?>