<?php

require_once('../inc/config.inc.php');
require_once(BASE_PATH . 'inc/ocr.inc.php');

$i = 0;

$numbers = array();
$stars = array();

foreach (OCR\numbersForFileNamed('numbers.png') as $number) {
	if ($i < 5)
		$numbers[] = intval($number);
	else
		$stars[] = intval($number);
	
	$i++;
}

echo json_encode(array('numbers' => $numbers, 'stars' => $stars));

?>