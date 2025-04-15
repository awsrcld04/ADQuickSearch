
# ADQuickSearch

DESCRIPTION: 
- Find user accounts/groups/computers

> NOTES: Code in initial repo commit is from 2011. 

## Requirements:

Operating System Requirements:
- Windows Server 2003 or higher (32-bit)
- Windows Server 2008 or higher (32-bit)

Additional software requirements:

Microsoft .NET Framework v3.5

Additional requirements:

Administrative access is required to perform operations by ADQuickSearch


## Operation and Configuration:

Command-line parameters: (Use the following required parameters in the following order)
- run 
- name (specify the name to search for)
- user/group/computer (to specify the type of object)
- brief/full (to specify the output detail)

Example:

ADQuickSearch -run -name:TestUser1 -user -brief

ADQuickSearch -run -name:TestGroup1 -group -full

