//****************************************
// Copyright (c) Thinkability Group 2011
// SystemsAdminPro.com
//****************************************

// ADQuickSearch -run -name: -user -brief/full
// ADQuickSearch -run -name: -group -brief/full
// ADQuickSearch -run -name: -computer -brief/full

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using Microsoft.Win32;
using System.Reflection;
using SystemsAdminPro.Utility;

namespace ADQuickSearch
{
    class ADQSMain
    {
        struct SearchParams
        {
            public int intSearchObjectType; // 1 - user, 2 - group, 3 - computer
            public int intDetailLevel; // 1 - brief, 2 - full
            public string strName;
        }

        struct CMDArguments
        {
            public bool bParseCmdArguments;
            public string strName;
            public int intSearchObjectType;
            public int intDetailLevel;
        }

        static void funcPrintParameterSyntax()
        {
            Console.WriteLine("ADQuickSearch v1.0 (c) 2011 SystemsAdminPro.com");
            Console.WriteLine();
            Console.WriteLine("Description: Find user accounts/groups/computers");
            Console.WriteLine();
            Console.WriteLine("Parameter syntax:");
            Console.WriteLine();
            Console.WriteLine("Use the following required parameters in the following order:");
            Console.WriteLine("-run                     required parameter");
            Console.WriteLine("-name:                   to specify the name to search for");
            Console.WriteLine("-user/group/computer     to specify the type of object");
            Console.WriteLine("-brief/full              to specify the output detail");
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine("ADQuickSearch -run -name:TestUser1 -user -brief");
            Console.WriteLine("ADQuickSearch -run -name:TestGroup1 -group -full");
        }

        static CMDArguments funcParseCmdArguments(string[] cmdargs)
        {
            CMDArguments objCMDArguments = new CMDArguments();

            try
            {
                bool bCmdArg1Complete = false;
                bool bCmdArg2Complete = false;

                if (cmdargs[0] == "-run" & cmdargs.Length > 1)
                {
                    if (cmdargs[1].Contains("-name:"))
                    {
                        // [DebugLine] Console.WriteLine(cmdargs[1].Substring(6));
                        objCMDArguments.strName = cmdargs[1].Substring(6);
                        bCmdArg1Complete = true;

                        if (bCmdArg1Complete & cmdargs.Length > 2)
                        {
                            if (cmdargs[2] == "-user")
                            {
                                // [DebugLine] Console.WriteLine(cmdargs[2].Substring(5));
                                objCMDArguments.intSearchObjectType = 1;
                                bCmdArg2Complete = true;

                                if (bCmdArg2Complete & cmdargs.Length > 3)
                                {
                                    if (cmdargs[3] == "-brief")
                                    {
                                        // [DebugLine] Console.WriteLine(cmdargs[3].Substring(6));
                                        objCMDArguments.intDetailLevel = 1;
                                        objCMDArguments.bParseCmdArguments = true;
                                    }
                                    if (cmdargs[3] == "-full")
                                    {
                                        // [DebugLine] Console.WriteLine(cmdargs[3].Substring(6));
                                        objCMDArguments.intDetailLevel = 2;
                                        objCMDArguments.bParseCmdArguments = true;
                                    }
                                }
                            }
                            if (cmdargs[2] == "-group")
                            {
                                // [DebugLine] Console.WriteLine(cmdargs[2].Substring(5));
                                objCMDArguments.intSearchObjectType = 2;
                                bCmdArg2Complete = true;

                                if (bCmdArg2Complete & cmdargs.Length > 3)
                                {
                                    if (cmdargs[3] == "-brief")
                                    {
                                        // [DebugLine] Console.WriteLine(cmdargs[3].Substring(6));
                                        objCMDArguments.intDetailLevel = 1;
                                        objCMDArguments.bParseCmdArguments = true;
                                    }
                                    if (cmdargs[3] == "-full")
                                    {
                                        // [DebugLine] Console.WriteLine(cmdargs[3].Substring(6));
                                        objCMDArguments.intDetailLevel = 2;
                                        objCMDArguments.bParseCmdArguments = true;
                                    }
                                }
                            }
                            if (cmdargs[2] == "-computer")
                            {
                                // [DebugLine] Console.WriteLine(cmdargs[2].Substring(5));
                                objCMDArguments.intSearchObjectType = 3;
                                bCmdArg2Complete = true;

                                if (bCmdArg2Complete & cmdargs.Length > 3)
                                {
                                    if (cmdargs[3] == "-brief")
                                    {
                                        // [DebugLine] Console.WriteLine(cmdargs[3].Substring(6));
                                        objCMDArguments.intDetailLevel = 1;
                                        objCMDArguments.bParseCmdArguments = true;
                                    }
                                    if (cmdargs[3] == "-full")
                                    {
                                        // [DebugLine] Console.WriteLine(cmdargs[3].Substring(6));
                                        objCMDArguments.intDetailLevel = 2;
                                        objCMDArguments.bParseCmdArguments = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Construct.PrintParameterWarning(Construct.strProgramName);
                    objCMDArguments.bParseCmdArguments = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("tacf4: {0}", ex.Message);
            }

            return objCMDArguments;
        }

        static void funcProgramExecution(CMDArguments objCMDArguments2)
        {
            try
            {
                // [DebugLine] Console.WriteLine("Entering funcProgramExecution");

                SearchParams tmpSearchParams;

                tmpSearchParams.intSearchObjectType = objCMDArguments2.intSearchObjectType;
                tmpSearchParams.intDetailLevel = objCMDArguments2.intDetailLevel;
                tmpSearchParams.strName = objCMDArguments2.strName;

                Construct.ProgramRegistryTag(Construct.strProgramName);

                PrincipalContext ctxDomain = Construct.CreateDomainPrincipalContext();

                if (ctxDomain != null)
                {
                    if (objCMDArguments2.intSearchObjectType == 1)
                    {
                        funcSearchForUser(tmpSearchParams, ctxDomain);
                    }
                    if (objCMDArguments2.intSearchObjectType == 2)
                    {
                        funcSearchForGroup(tmpSearchParams, ctxDomain);
                    }
                    if (objCMDArguments2.intSearchObjectType == 3)
                    {
                        funcSearchForComputer(tmpSearchParams, ctxDomain);
                    }

                }
                else
                {
                    Console.WriteLine("No valid PrincipalContext.");
                }

            }
            catch (Exception ex)
            {
                // [DebugLine] Console.WriteLine(ex.Source);
                // [DebugLine] Console.WriteLine(ex.Message);
                // [DebugLine] Console.WriteLine(ex.StackTrace);

                Console.WriteLine("tacf5: {0}", ex.Message);
            }

        }

        static void funcSearchForUser(SearchParams newSearchParams, PrincipalContext currentctx)
        {
            UserPrincipal newUserPrincipal = new UserPrincipal(currentctx);

            PrincipalSearcher ps = new PrincipalSearcher(newUserPrincipal);

            // Create an in-memory user object to use as the query example.
            UserPrincipal u = new UserPrincipal(currentctx);

            // Set properties on the user principal object.
            //u.GivenName = "Jim";
            //u.Surname = "Daly";
            u.Name = newSearchParams.strName;

            // Tell the PrincipalSearcher what to search for.
            ps.QueryFilter = u;

            // Run the query. The query locates users 
            // that match the supplied user principal object. 
            Principal newPrincipal = ps.FindOne();

            if (newPrincipal != null)
            {
                newUserPrincipal = UserPrincipal.FindByIdentity(currentctx, IdentityType.Name, newPrincipal.Name);

                if (newSearchParams.intDetailLevel == 1)
                {
                    Console.WriteLine("Name: {0}", newUserPrincipal.Name);
                    Console.WriteLine("DisplayName: {0}", newUserPrincipal.DisplayName);
                    Console.WriteLine("Description: {0}", newUserPrincipal.Description);
                    if (newUserPrincipal.Enabled.ToString() == "True")
                    {
                        Console.WriteLine("Enabled: Yes");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Enabled: No");
                        Console.ResetColor();
                    }
                }
                if (newSearchParams.intDetailLevel == 2)
                {
                    Console.WriteLine("Name: {0}", newUserPrincipal.Name);
                    Console.WriteLine("SID: {0}", newUserPrincipal.Sid);
                    Console.WriteLine("DisplayName: {0}", newUserPrincipal.DisplayName);
                    Console.WriteLine("Description: {0}", newUserPrincipal.Description);
                    if (newUserPrincipal.Enabled.ToString() == "True")
                    {
                        Console.WriteLine("Enabled: Yes");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Enabled: No");
                        Console.ResetColor();
                    }
                    Console.WriteLine("Logon name: {0}", newUserPrincipal.SamAccountName);
                    Console.WriteLine("DN: {0}", newUserPrincipal.DistinguishedName);
                }
            }
            else
            {
                Console.WriteLine("User {0} could not be found. Try again.", newSearchParams.strName);
            }

        }

        static void funcSearchForGroup(SearchParams newSearchParams, PrincipalContext currentctx)
        {
            GroupPrincipal newGroupPrincipal = new GroupPrincipal(currentctx);

            PrincipalSearcher ps = new PrincipalSearcher(newGroupPrincipal);

            // Create an in-memory user object to use as the query example.
            GroupPrincipal g = new GroupPrincipal(currentctx);

            // Set properties on the user principal object.
            //u.GivenName = "Jim";
            //u.Surname = "Daly";
            g.Name = newSearchParams.strName;

            // Tell the PrincipalSearcher what to search for.
            ps.QueryFilter = g;

            // Run the query. The query locates users 
            // that match the supplied user principal object. 
            Principal newPrincipal = ps.FindOne();

            if (newPrincipal != null)
            {
                newGroupPrincipal = GroupPrincipal.FindByIdentity(currentctx, IdentityType.Name, newPrincipal.Name);

                if (newSearchParams.intDetailLevel == 1)
                {
                    Console.WriteLine("Name: {0}", newGroupPrincipal.Name);
                    Console.WriteLine("DisplayName: {0}", newGroupPrincipal.DisplayName);
                    Console.WriteLine("Description: {0}", newGroupPrincipal.Description);
                    if (newGroupPrincipal.IsSecurityGroup.ToString() == "True")
                    {
                        Console.WriteLine("Security Group: Yes");
                    }
                    else
                    {
                        Console.WriteLine("Security Group: No");
                    }
                }
                if (newSearchParams.intDetailLevel == 2)
                {
                    Console.WriteLine("Name: {0}", newGroupPrincipal.Name);
                    Console.WriteLine("SID: {0}", newGroupPrincipal.Sid);
                    Console.WriteLine("DisplayName: {0}", newGroupPrincipal.DisplayName);
                    Console.WriteLine("Description: {0}", newGroupPrincipal.Description);
                    if (newGroupPrincipal.IsSecurityGroup.ToString() == "True")
                    {
                        Console.WriteLine("Security Group: Yes");
                    }
                    else
                    {
                        Console.WriteLine("Security Group: No");
                    }
                    Console.WriteLine("Logon name: {0}", newGroupPrincipal.SamAccountName);
                    Console.WriteLine("DN: {0}", newGroupPrincipal.DistinguishedName);
                }
            }
            else
            {
                Console.WriteLine("Group {0} could not be found. Try again.", newSearchParams.strName);
            }
        }

        static void funcSearchForComputer(SearchParams newSearchParams, PrincipalContext currentctx)
        {
            ComputerPrincipal newComputerPrincipal = new ComputerPrincipal(currentctx);

            PrincipalSearcher ps = new PrincipalSearcher(newComputerPrincipal);

            // Create an in-memory user object to use as the query example.
            ComputerPrincipal c = new ComputerPrincipal(currentctx);

            // Set properties on the user principal object.
            //u.GivenName = "Jim";
            //u.Surname = "Daly";
            c.Name = newSearchParams.strName;

            // Tell the PrincipalSearcher what to search for.
            ps.QueryFilter = c;

            // Run the query. The query locates users 
            // that match the supplied user principal object. 
            Principal newPrincipal = ps.FindOne();

            if (newPrincipal != null)
            {
                newComputerPrincipal = ComputerPrincipal.FindByIdentity(currentctx, IdentityType.Name, newPrincipal.Name);

                if (newSearchParams.intDetailLevel == 1)
                {
                    Console.WriteLine("Name: {0}", newComputerPrincipal.Name);
                    if (newComputerPrincipal.Name != newComputerPrincipal.DisplayName)
                        Console.WriteLine("DisplayName: {0}", newComputerPrincipal.DisplayName);
                    Console.WriteLine("Description: {0}", newComputerPrincipal.Description);
                    if (newComputerPrincipal.Enabled.ToString() == "True")
                    {
                        Console.WriteLine("Enabled: Yes");
                    }
                    else
                    {
                        Console.WriteLine("Enabled: No");
                    }
                }
                if (newSearchParams.intDetailLevel == 2)
                {
                    Console.WriteLine("Name: {0}", newComputerPrincipal.Name);
                    Console.WriteLine("SID: {0}", newComputerPrincipal.Sid);
                    if(newComputerPrincipal.Name != newComputerPrincipal.DisplayName)
                        Console.WriteLine("DisplayName: {0}", newComputerPrincipal.DisplayName);
                    Console.WriteLine("Description: {0}", newComputerPrincipal.Description);
                    if (newComputerPrincipal.Enabled.ToString() == "True")
                    {
                        Console.WriteLine("Enabled: Yes");
                    }
                    else
                    {
                        Console.WriteLine("Enabled: No");
                    }
                    Console.WriteLine("Logon name: {0}", newComputerPrincipal.SamAccountName);
                    Console.WriteLine("DN: {0}", newComputerPrincipal.DistinguishedName);
                }
            }
            else
            {
                Console.WriteLine("Computer {0} could not be found. Try again.", newSearchParams.strName);
            }
        }

        static void funcGetFuncCatchCode(string strFunctionName, Exception currentex)
        {
            string strCatchCode = "";

            Dictionary<string, string> dCatchTable = new Dictionary<string, string>();
            dCatchTable.Add("funcGetFuncCatchCode", "p:f0");
            dCatchTable.Add("funcPrintParameterSyntax", "p:f1");
            dCatchTable.Add("funcParseCmdArguments", "p:f2");
            dCatchTable.Add("funcProgramExecution", "p:f3");
            dCatchTable.Add("funcSearchForUser", "p:f4");
            dCatchTable.Add("funcSearchForGroup", "p:f5");
            dCatchTable.Add("funcSearchForComputer", "p:f6");

            if (dCatchTable.ContainsKey(strFunctionName))
            {
                strCatchCode = "err" + dCatchTable[strFunctionName] + ": ";
            }

            //[DebugLine] Console.WriteLine(strCatchCode + currentex.GetType().ToString());
            //[DebugLine] Console.WriteLine(strCatchCode + currentex.Message);

            Construct.WriteToErrorLogFile(strCatchCode + currentex.GetType().ToString());
            Construct.WriteToErrorLogFile(strCatchCode + currentex.Message);

        }

        static void Main(string[] args)
        {
            try
            {
                Construct.strProgramName = "ADQuickSearch";

                //if (funcLicenseCheck())
                if (Construct.LicenseActivation())
                {
                    if (args.Length == 0)
                    {
                        Construct.PrintParameterWarning(Construct.strProgramName);
                    }
                    else
                    {
                        if (args[0] == "-?")
                        {
                            funcPrintParameterSyntax();
                        }
                        else
                        {
                            string[] arrArgs = args;
                            CMDArguments objArgumentsProcessed = funcParseCmdArguments(arrArgs);

                            if (objArgumentsProcessed.bParseCmdArguments)
                            {
                                funcProgramExecution(objArgumentsProcessed);
                            }
                            else
                            {
                                Construct.PrintParameterWarning(Construct.strProgramName);
                            } // check objArgumentsProcessed.bParseCmdArguments
                        } // check args[0] = "-?"
                    } // check args.Length == 0
                } // funcLicenseCheck()
            }
            catch (Exception ex)
            {
                Console.WriteLine("errm0: {0}", ex.Message);
            }
        }
    }
}
