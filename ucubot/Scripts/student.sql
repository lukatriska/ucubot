use ucubot;

CREATE TABLE student (
  id INT NOT NULL AUTO_INCREMENT,
	first_name VARCHAR(255),
	last_name VARCHAR(255),
	user_id UNIQUE VARCHAR(255),
	PRIMARY KEY(id)
)