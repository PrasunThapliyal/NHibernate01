
    
alter table onep_amptp  drop foreign key FK_298A3252


    
alter table onep_terminationpoint  drop foreign key FK_47A14A86


    
alter table onep_terminationpoint  drop foreign key FK_353543FA


    
alter table onep_topologicallink  drop foreign key FK_E6468C8D


    
alter table onep_topologicallink  drop foreign key FK_C07BFEF1


    
alter table onep_topologicallink  drop foreign key FK_33EE3BAB


    
alter table onep_topologicallink  drop foreign key FK_1588FBB9


    
alter table onep_fibertl  drop foreign key FK_DBFE8370


    
alter table onep_fibertl  drop foreign key FK_D775C7F5


    drop table if exists onep_amptp

    drop table if exists onep_network

    drop table if exists onep_terminationpoint

    drop table if exists onep_topologicallink

    drop table if exists onep_fibertl

    drop table if exists hibernate_unique_key

    create table onep_amptp (
        oid INTEGER UNSIGNED not null,
       targetGain DOUBLE,
       network INTEGER UNSIGNED,
       primary key (oid)
    )

    create table onep_network (
        oid INTEGER UNSIGNED not null,
       name VARCHAR(255),
       mcpProjectId VARCHAR(255),
       primary key (oid)
    )

    create table onep_terminationpoint (
        oid INTEGER UNSIGNED not null,
       PTP TINYINT UNSIGNED,
       name VARCHAR(255),
       notes VARCHAR(255),
       network INTEGER UNSIGNED,
       OnepAmpRole INTEGER UNSIGNED unique,
       primary key (oid)
    )

    create table onep_topologicallink (
        oid INTEGER UNSIGNED not null,
       Discriminator INTEGER,
       name VARCHAR(255),
       length DOUBLE,
       uniMate INTEGER UNSIGNED,
       aEndTP INTEGER UNSIGNED,
       zEndTP INTEGER UNSIGNED,
       network INTEGER UNSIGNED,
       primary key (oid)
    )

    create table onep_fibertl (
        oid INTEGER UNSIGNED not null,
       loss DOUBLE,
       network INTEGER UNSIGNED,
       primary key (oid)
    )

    alter table onep_amptp 
        add index (network), 
        add constraint FK_298A3252 
        foreign key (network) 
        references onep_network (oid)

    alter table onep_terminationpoint 
        add index (network), 
        add constraint FK_47A14A86 
        foreign key (network) 
        references onep_network (oid)

    alter table onep_terminationpoint 
        add index (OnepAmpRole), 
        add constraint FK_353543FA 
        foreign key (OnepAmpRole) 
        references onep_amptp (oid)

    alter table onep_topologicallink 
        add index (uniMate), 
        add constraint FK_E6468C8D 
        foreign key (uniMate) 
        references onep_topologicallink (oid)

    alter table onep_topologicallink 
        add index (aEndTP), 
        add constraint FK_C07BFEF1 
        foreign key (aEndTP) 
        references onep_terminationpoint (oid)

    alter table onep_topologicallink 
        add index (zEndTP), 
        add constraint FK_33EE3BAB 
        foreign key (zEndTP) 
        references onep_terminationpoint (oid)

    alter table onep_topologicallink 
        add index (network), 
        add constraint FK_1588FBB9 
        foreign key (network) 
        references onep_network (oid)

    alter table onep_fibertl 
        add index (oid), 
        add constraint FK_DBFE8370 
        foreign key (oid) 
        references onep_topologicallink (oid)

    alter table onep_fibertl 
        add index (network), 
        add constraint FK_D775C7F5 
        foreign key (network) 
        references onep_network (oid)

    create table hibernate_unique_key (
         next_hi INTEGER 
    )

    insert into hibernate_unique_key values ( 1 )
