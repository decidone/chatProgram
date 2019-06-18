CREATE DATABASE chat_program;

USE chat_program;

CREATE TABLE user(
    user_id VARCHAR(15) NOT NULL,
    user_pw VARCHAR(15) NOT NULL,
    user_name VARCHAR(15)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE chat_room(
    room_num INT NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE chat_user(
    room_num INT NOT NULL,
    user_id VARCHAR(15) NOT NULL,
    user_in_room BOOLEAN,
    last_chat INT
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE friend(
    user_id VARCHAR(15) NOT NULL,
    friend_id VARCHAR(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE chat(
    chat_num INT NOT NULL,
    room_num INT NOT NULL,
    user_id VARCHAR(15) NOT NULL,
    chat_message VARCHAR(80),
    chat_time DATETIME
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

ALTER TABLE user ADD PRIMARY KEY(user_id);
ALTER TABLE `chat_room` MODIFY `room_num` INT AUTO_INCREMENT PRIMARY KEY;
ALTER TABLE `chat` MODIFY `chat_num` INT AUTO_INCREMENT PRIMARY KEY;
ALTER TABLE chat_user ADD PRIMARY KEY(room_num, user_id);
ALTER TABLE friend ADD PRIMARY KEY(user_id, friend_id);

ALTER TABLE chat_user ADD CONSTRAINT chat_fk1 FOREIGN KEY(room_num) REFERENCES chat_room(room_num);
ALTER TABLE chat_user ADD CONSTRAINT chat_fk2 FOREIGN KEY(user_id) REFERENCES user(user_id);
-- ALTER TABLE chat_user ADD CONSTRAINT chat_fk3 FOREIGN KEY(last_chat) REFERENCES chat(chat_num);
ALTER TABLE friend ADD CONSTRAINT chat_fk4 FOREIGN KEY(user_id) REFERENCES user(user_id);
ALTER TABLE friend ADD CONSTRAINT chat_fk5 FOREIGN KEY(friend_id) REFERENCES user(user_id);
ALTER TABLE chat ADD CONSTRAINT chat_fk6 FOREIGN KEY(room_num) REFERENCES chat_room(room_num);
ALTER TABLE chat ADD CONSTRAINT chat_fk7 FOREIGN KEY(user_id) REFERENCES user(user_id);
