<?php

define('EURO_MILHOES_ENDPOINT', 'https://nunofcguerreiro.com/api-euromillions-json');

define('PUSH_MESSAGE', 'Abra a aplicação para conferir qual o seu prémio!');

require_once('../inc/config.inc.php');

require_once(BASE_PATH . 'inc/database.inc.php');
require_once(BASE_PATH . 'inc/push.inc.php');

$lastUpdateDate = '1970-01-01';

$lastDraw = @$db_conn->query("SELECT * FROM `draws` ORDER BY `id` DESC LIMIT 1")->fetchAll()[0];

if ($lastDraw)
	$lastUpdateDate = $lastDraw['date'];

$ch = curl_init();

curl_setopt($ch, CURLOPT_URL, EURO_MILHOES_ENDPOINT);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);

$content = curl_exec($ch);

curl_close($ch);

$content = json_decode($content, true);

$obj = $content['drawns'][0];

$lastDrawDate = $obj['date'];

$dt1 = new DateTime($lastUpdateDate);
$dt2 = new DateTime($lastDrawDate);

if ($dt2 > $dt1) {
	$arr = array(	'numbers' => array_map('intval', 
								explode(' ', $obj['numbers'])), 
					'stars' => array_map('intval', 
								explode(' ', $obj['stars']))
				);
	
	$db_conn->query("INSERT INTO `draws` (`draw`, `date`) VALUES (%s, %s)", json_encode($arr), $obj['date']);
	
	echo 'Updated';
	
	$iOS = new iOSPush(PUSH_MESSAGE);
	
	$iOS->setDevices(Push::getTokens(DEVICE_TYPE_IOS));
	
	$iOS->send();
	
	$android = new AndroidPush(PUSH_MESSAGE);
	
	$android->setDevices(Push::getTokens(DEVICE_TYPE_ANDROID));
	
	$android->send();
}

?>