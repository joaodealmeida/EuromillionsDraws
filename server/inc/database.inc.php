<?php

class pdo_mysql_manager {
    private $connection = null;

    function connect_to_database($hostname, $db_name, $username, $password, $persistent = false, $fetch_mode = PDO::FETCH_ASSOC) {
        $this->close_database();

        $options = array(PDO::ATTR_PERSISTENT => $persistent, PDO::ATTR_DEFAULT_FETCH_MODE => $fetch_mode, PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES utf8mb4");

        $this->connection = new PDO("mysql:host={$hostname};dbname={$db_name};charset=utf8mb4", $username, $password, $options);
        $this->connection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

        if (!($this->connection))
            error("Database Connection Failed...");
    }

    function close_database() {
        if ($this->connection)
            $this->connection = null;
    }

    function escape_string($the_string) {
        return $this->connection->quote($the_string);
    }

    function quote_object($the_object) {
        if (is_null($the_object)) {
            return "''";
        } else if (is_string($the_object)) {
            return $this->escape_string($the_object);
        } else if (is_float($the_object) || is_integer($the_object)) {
            return $the_object;
        } else if (is_bool($the_object)) {
            return ($the_object ? 1 : 0);
        }

        return "''";
    }

    function query($format) {
        $result = NULL;

        try {
            if (isset($this->connection)) {
                $args = func_get_args();
                array_shift($args);
                $args = array_map(array($this, "quote_object"), $args);
                $query = vsprintf($format, $args);

                $result = $this->connection->query($query);
            }
        } catch (Exception $e) {
            die("Error: " . $e->getMessage());
        }

        return $result;
    }

    function last_id() {
        $result = 0;

        if (isset($this->connection)) {
            $result = $this->connection->lastInsertId();
        }

        return $result;
    }
}
 
$db_host = "localhost";
$db_user = "sdisem";
$db_pass = "AUDzAJJR7NVaH82R";
$db_name = "sdisem";
 
$db_conn = new pdo_mysql_manager;
 
$db_conn->connect_to_database($db_host, $db_name, $db_user, $db_pass);

?>