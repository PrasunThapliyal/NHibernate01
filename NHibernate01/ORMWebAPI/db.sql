
    
alter table onep_amptp  drop foreign key FK_C4980009


    
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
