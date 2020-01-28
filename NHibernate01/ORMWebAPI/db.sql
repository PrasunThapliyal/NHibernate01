
    
alter table ActorRole  drop foreign key FK_B3337C3E


    
alter table Book  drop foreign key FK_2E5EFA32


    
alter table Movie  drop foreign key FK_13C98C6D


    drop table if exists ActorRole

    drop table if exists product

    drop table if exists Book

    drop table if exists Movie

    drop table if exists hibernate_unique_key

    create table ActorRole (
        id INTEGER not null,
       Actor VARCHAR(255) not null,
       Role VARCHAR(255) not null,
       MovieId INTEGER,
       ActorIndex INTEGER,
       primary key (id)
    )

    create table product (
        id INTEGER not null,
       Name VARCHAR(255) not null,
       description VARCHAR(100),
       UnitPrice NUMERIC(18,4) not null,
       primary key (id)
    )

    create table Book (
        Id INTEGER not null,
       ISBN VARCHAR(255),
       Author VARCHAR(255),
       primary key (Id)
    )

    create table Movie (
        Id INTEGER not null,
       Director VARCHAR(255),
       NewProp VARCHAR(100),
       primary key (Id)
    )

    alter table ActorRole 
        add index (MovieId), 
        add constraint FK_B3337C3E 
        foreign key (MovieId) 
        references Movie (Id)

    alter table Book 
        add index (Id), 
        add constraint FK_2E5EFA32 
        foreign key (Id) 
        references product (id)

    alter table Movie 
        add index (Id), 
        add constraint FK_13C98C6D 
        foreign key (Id) 
        references product (id)

    create table hibernate_unique_key (
         next_hi INTEGER 
    )

    insert into hibernate_unique_key values ( 1 )
