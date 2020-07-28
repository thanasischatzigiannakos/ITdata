# ITdata

## A user and resources management program developed during my internship.



### Main uses
___ 
 
The applications purpose is to allow the IT department of the company(the company I worked at manages the IT side of a factory and subsequently everyone and everything involed with it) to manage efficiently the data from employees(e-mails,company phones,personal information)
,hardware used in the factory(switches,printers, phones) and test the status of various servers in the network.

Specifically the applications user(IT admin) can add,edit or delete an employee,a phone,or a printer from the database. Furthermore since the IT department, the application is developed for, might need to manage resources from different companies(*at the moment there are 3 seperate companies they are managing in the factory*) the admins are able to add edit or delete companies,locations and departments from the database.(_The database used was created specifically for this application and does not change the data the rest of the factory have access to).



### Entities and interconnections


At the moment the database contains 7 tables which are: 

+ users
+ mails
+ user_mail
+ phones
+ companies
+ location
+ department



#### _*Database Tables rundown*_


*The users table contains:* 

      1.first name
      2.last name
      3.greek first name
      4.greek last name
      5. company ID(foreign key) 
      5. location ID(foreign key) 
      6. department id(foreign key)
      7.Job Description
      8.hostname
      9.username
      10.password
      11.admin_password(if the user is an admin)
      12.status
      13.radmin port
      14.notes
      
*The mails table contains:*

     1.id
     2.email
     3.password
      


*The user_mail table contains:*

    1.user_id(primary key,foreign key)
    2.mail_id(primary key, foreign key)
    


*The phones table contains:*

    1.id
    2.model
    3.serial number
    4.IP
    5.MAC Address
    6.Internal number
    7.External Number
    8.Web User
    9.Web Password
    10.User ID(foreign key)
    
    
    
*The companies table contains:*

    1.id
    2.company(the companys name)
    


*The location table contains:*

    1.id
    2.location(name of the location) 
    
    
    
*The department table contains:*

    1.id
    2.department(the departments name)

