USE ucubot;
CREATE TABLE lesson_signal (
	id INT NOT NULL AUTO_INCREMENT,
	time_stamp TIMESTAMP,
	signal_type INT,
  ALTER TABLE lesson_signal ADD
    COLUMN student_id UNIQUE VARCHAR(255),
    ADD FOREIGN KEY id(student_id) REFERENCES student(user_id) ON DELETE RESTRICT ON UPDATE CASCADE;
  PRIMARY KEY(id)
);
