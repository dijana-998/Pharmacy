-- MySQL Workbench Synchronization
-- Generated: 2024-11-19 15:00
-- Model: New Model
-- Version: 1.0
-- Project: Name of the project
-- Author: Lenovo

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

ALTER SCHEMA `apoteka`  DEFAULT COLLATE utf8_unicode_ci ;

ALTER TABLE `apoteka`.`racun` 
DROP FOREIGN KEY `fk_racun_zaposleni1`;

ALTER TABLE `apoteka`.`stavka_racuna` 
DROP FOREIGN KEY `fk_stavka_racuna_artikal1`;

ALTER TABLE `apoteka`.`stavka_narudzbe` 
DROP FOREIGN KEY `fk_stavka_narudzbe_artikal1`;

ALTER TABLE `apoteka`.`narudzba` 
DROP FOREIGN KEY `fk_narudzba_dobavljac1`;

ALTER TABLE `apoteka`.`zaposleni` 
DROP FOREIGN KEY `fk_zaposleni_jezik1`;

ALTER TABLE `apoteka`.`racun` 
COLLATE = utf8_unicode_ci ;

ALTER TABLE `apoteka`.`stavka_racuna` 
COLLATE = utf8_unicode_ci ;

ALTER TABLE `apoteka`.`artikal` 
COLLATE = utf8_unicode_ci ,
CHANGE COLUMN `naziv` `naziv` VARCHAR(50) NULL DEFAULT NULL ,
CHANGE COLUMN `opis` `opis` VARCHAR(500) NULL DEFAULT NULL ;

ALTER TABLE `apoteka`.`stavka_narudzbe` 
COLLATE = utf8_unicode_ci ;

ALTER TABLE `apoteka`.`narudzba` 
COLLATE = utf8_unicode_ci ;

ALTER TABLE `apoteka`.`dobavljac` 
COLLATE = utf8_unicode_ci ,
CHANGE COLUMN `naziv` `naziv` VARCHAR(50) NULL DEFAULT NULL ,
CHANGE COLUMN `telefon` `telefon` VARCHAR(45) NULL DEFAULT NULL ,
CHANGE COLUMN `adresa` `adresa` VARCHAR(70) NULL DEFAULT NULL ,
CHANGE COLUMN `email` `email` VARCHAR(45) NULL DEFAULT NULL ;

ALTER TABLE `apoteka`.`zaposleni` 
COLLATE = utf8_unicode_ci ,
ADD COLUMN `jezik` VARCHAR(45) NULL DEFAULT NULL AFTER `datum_prestanka`,
CHANGE COLUMN `jmb` `jmb` VARCHAR(13) NULL DEFAULT NULL ,
CHANGE COLUMN `ime` `ime` VARCHAR(50) NULL DEFAULT NULL ,
CHANGE COLUMN `prezime` `prezime` VARCHAR(50) NULL DEFAULT NULL ,
CHANGE COLUMN `korisnicko_ime` `korisnicko_ime` VARCHAR(50) NULL DEFAULT NULL ,
CHANGE COLUMN `lozinka` `lozinka` VARCHAR(45) NULL DEFAULT NULL ,
CHANGE COLUMN `uloga` `uloga` VARCHAR(45) NULL DEFAULT NULL ;

ALTER TABLE `apoteka`.`tema` 
COLLATE = utf8_unicode_ci ,
CHANGE COLUMN `naziv` `naziv` VARCHAR(45) NULL DEFAULT NULL ;

ALTER TABLE `apoteka`.`jezik` 
COLLATE = utf8_unicode_ci ,
CHANGE COLUMN `naziv` `naziv` VARCHAR(45) NULL DEFAULT NULL ;

ALTER TABLE `apoteka`.`racun` 
ADD CONSTRAINT `fk_racun_zaposleni1`
  FOREIGN KEY (`zaposleni_idzaposleni`)
  REFERENCES `apoteka`.`zaposleni` (`idzaposleni`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `apoteka`.`stavka_racuna` 
DROP FOREIGN KEY `fk_stavka_racuna_racun`;

ALTER TABLE `apoteka`.`stavka_racuna` ADD CONSTRAINT `fk_stavka_racuna_racun`
  FOREIGN KEY (`racun_idracun`)
  REFERENCES `apoteka`.`racun` (`idracun`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_stavka_racuna_artikal1`
  FOREIGN KEY (`artikal_idartikal`)
  REFERENCES `apoteka`.`artikal` (`idartikal`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `apoteka`.`stavka_narudzbe` 
DROP FOREIGN KEY `fk_stavka_narudzbe_narudzba1`;

ALTER TABLE `apoteka`.`stavka_narudzbe` ADD CONSTRAINT `fk_stavka_narudzbe_narudzba1`
  FOREIGN KEY (`narudzba_idnarudzba`)
  REFERENCES `apoteka`.`narudzba` (`idnarudzba`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_stavka_narudzbe_artikal1`
  FOREIGN KEY (`artikal_idartikal`)
  REFERENCES `apoteka`.`artikal` (`idartikal`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `apoteka`.`narudzba` 
ADD CONSTRAINT `fk_narudzba_dobavljac1`
  FOREIGN KEY (`dobavljac_iddobavljac`)
  REFERENCES `apoteka`.`dobavljac` (`iddobavljac`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `apoteka`.`zaposleni` 
DROP FOREIGN KEY `fk_zaposleni_tema1`;

ALTER TABLE `apoteka`.`zaposleni` ADD CONSTRAINT `fk_zaposleni_tema1`
  FOREIGN KEY (`tema_idtema`)
  REFERENCES `apoteka`.`tema` (`idtema`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_zaposleni_jezik1`
  FOREIGN KEY (`jezik_idjezik`)
  REFERENCES `apoteka`.`jezik` (`idjezik`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;


DELIMITER $$

USE `apoteka`$$
DROP TRIGGER IF EXISTS `apoteka`.`smanji_kolicinu` $$

USE `apoteka`$$
DROP TRIGGER IF EXISTS `apoteka`.`povecaj_kolicinu` $$


DELIMITER ;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
