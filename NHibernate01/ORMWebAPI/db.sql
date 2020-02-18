
    
alter table ActorRole  drop foreign key FK_767D4F64


    
alter table book  drop foreign key FK_118C435D


    
alter table movie  drop foreign key FK_4449E6F4


    drop table if exists ActorRole

    drop table if exists product

    drop table if exists book

    drop table if exists movie

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
       Discriminator INTEGER,
       Name VARCHAR(255) not null,
       description VARCHAR(100),
       UnitPrice NUMERIC(18,4) not null,
       primary key (id)
    )

    create table book (
        Id INTEGER not null,
       ISBN VARCHAR(255),
       Author VARCHAR(255),
       primary key (Id)
    )

    create table movie (
        Id INTEGER not null,
       Director VARCHAR(255),
       NewProp VARCHAR(100),
       primary key (Id)
    )

    alter table ActorRole 
        add index (MovieId), 
        add constraint FK_767D4F64 
        foreign key (MovieId) 
        references product (id)

    alter table book 
        add index (Id), 
        add constraint FK_118C435D 
        foreign key (Id) 
        references product (id)

    alter table movie 
        add index (Id), 
        add constraint FK_4449E6F4 
        foreign key (Id) 
        references product (id)

    create table hibernate_unique_key (
         next_hi INTEGER 
    )

    insert into hibernate_unique_key values ( 1 )
