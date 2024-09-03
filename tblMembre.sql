drop table if exists Membre;
create table Membre (
Id int primary key,
Pseudo varchar(100),
Courriel varchar(100),
Username varchar(100),
Motdepasse varchar(100),
Role varchar(50),
profil varchar(150)
);