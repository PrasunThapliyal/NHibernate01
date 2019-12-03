
    drop table if exists student

    drop table if exists Teacher

    drop table if exists hibernate_unique_key

    create table student (
        id INTEGER not null,
       firstname VARCHAR(1000),
       lastname VARCHAR(255),
       primary key (id)
    )

    create table Teacher (
        Id INTEGER not null,
       Firstname VARCHAR(255),
       Lastname VARCHAR(255),
       primary key (Id)
    )

    create table hibernate_unique_key (
         next_hi INTEGER 
    )

    insert into hibernate_unique_key values ( 1 )
