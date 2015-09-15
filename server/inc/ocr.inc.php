<?php

namespace OCR;

require_once(BASE_PATH . 'lib/TesseractOCR.php');
require_once(BASE_PATH . 'inc/database.inc.php');

function numbersForFileNamed($fileName) {
	$tesseract = new \TesseractOCR(BASE_PATH . 'inbox/' . $fileName);
	
	$tesseract->setWhitelist(range(0, 9));
	
	return preg_split('/[ \n]/',  $tesseract->recognize());
}

function __join_paths() {
	$paths = array();

	foreach (func_get_args() as $arg)
		if ($arg !== '')
			$paths[] = $arg;

	return preg_replace('#/+#','/',join('/', $paths));
}

function storeRequest($user, $blob) {
	global $db_conn;
	
	try  {
		$db_conn->query("INSERT INTO `ocr` (`user`, `blob`) VALUES (%s, %s)", $user, $blob);
		
		$row = $db_conn->query("SELECT `id` FROM `ocr` WHERE `user` = %s", $user)->fetch();
		
		return $row['id'];
	} catch (Exception $e) {
		return false;
	}
}

function performJob($id) {
	global $db_conn;
	
	$result = $db_conn->query("SELECT * FROM `ocr` WHERE `id` = %s", $id)->fetchAll();
	
	foreach ($result as $row) {
		$blob = $row['blob'];
		
		$decoded = base64_decode($blob);
		
		$path = __join_paths(sys_get_temp_dir(), sha1($blob));
		
		file_put_contents($path, $decoded);
		
		$i = 0;
		
		$numbers = array();
		$stars = array();
		
		foreach (numbersForFileNamed($path) as $number) {
			if ($i < 5)
				$numbers[] = intval($number);
			else
				$stars[] = intval($number);
		
			$i++;
		}
		
		$jsonResult = json_encode(array('numbers' => $numbers, 'stars' => $stars));
		
		$db_conn->query("UPDATE `ocr` SET `result` = %s WHERE `id` = %s", $jsonResult, $id);
	}
}

function getRequest($user, $id, &$errorCode) {
	global $db_conn;
	
	$result = $db_conn->query("SELECT * FROM `ocr` WHERE `id` = %s AND `user` = %s", $id, $user);
	
	if (!$result->rowCount()) {
		$errorCode = 1;
		
		return false;
	}
	
	$row = $result->fetch()[0];
	
	if ($row['result'] && $row['result'] != "") {
		return $row['result'];
	} else {
		$errorCode = 2;
		
		return false;
	}
}

function deleteRequest($user, $id) {
	global $db_conn;
	
	$result = $db_conn->query("SELECT * FROM `ocr` WHERE `id` = %s AND `user` = %s", $id, $user);
	
	if (!$result->rowCount())
		return false;
	
	$db_conn->query("DELETE FROM `ocr` WHERE `id` = %s AND `user` = %s", $id, $user);
	
	return true;
}

?>