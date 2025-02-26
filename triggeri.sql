DELIMITER //

CREATE TRIGGER povecaj_kolicinu
AFTER INSERT ON stavka_narudzbe
FOR EACH ROW
BEGIN
    UPDATE artikal 
    SET kolicina = kolicina + NEW.kolicina
    WHERE idartikal = NEW.artikal_idartikal;
END //

DELIMITER ;

DELIMITER //

CREATE TRIGGER smanji_kolicinu
AFTER INSERT ON stavka_racuna
FOR EACH ROW
BEGIN
    UPDATE artikal 
    SET kolicina = kolicina - NEW.kolicina
    WHERE idartikal = NEW.artikal_idartikal;
END //

DELIMITER ;