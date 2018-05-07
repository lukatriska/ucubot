USE ucubot;
ALTER TABLE lesson_signal 
  DROP COLUMN user_id;
  ADD COLUMN student_id INT NOT NULL,
  ADD FOREIGN KEY student_id REFERENCES student(id) 
  ON DELETE RESTRICT 
  ON UPDATE RESTRICT;
