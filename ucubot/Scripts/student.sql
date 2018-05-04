use ucubot;

CREATE TABLE Student (
  id INT NOT NULL AUTO_INCREMENT,
	firstname VARCHAR(255),
	lastname VARCHAR(255),
	user_id UNIQUE VARCHAR(255),
	PRIMARY KEY(id)
)