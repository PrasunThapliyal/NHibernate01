
    drop table if exists product

    drop table if exists hibernate_unique_key

    create table product (
        id INTEGER not null,
       Name VARCHAR(255) not null,
       description VARCHAR(100),
       UnitPrice NUMERIC(18,4) not null,
       primary key (id)
    )

    create table hibernate_unique_key (
         next_hi INTEGER 
    )

    insert into hibernate_unique_key values ( 1 )
