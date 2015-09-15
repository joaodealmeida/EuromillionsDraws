<?php

require_once('../inc/config.inc.php');
require_once('../inc/ocr.inc.php');

$elapsed = 0;

while (true) {
	$time_start = microtime(true);
	
	$result = $db_conn->query("SELECT `id` FROM `ocr` ORDER BY `id` ASC LIMIT 1")->fetchAll();
	
	foreach ($result as $row)
		OCR\performJob($row['id']);
	
	$time_end = microtime(true);
	
	$elapsed += $time_end - $time_start;
	
	if ($elapsed > 60)
		exit;
}

?>