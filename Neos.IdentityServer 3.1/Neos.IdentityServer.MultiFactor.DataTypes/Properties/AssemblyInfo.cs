//******************************************************************************************************************************************************************************************//
// Copyright (c) 2023 redhook (adfsmfa@gmail.com)                                                                                                                                    //                        
//                                                                                                                                                                                          //
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),                                       //
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,   //
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:                                                                                   //
//                                                                                                                                                                                          //
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                                                           //
//                                                                                                                                                                                          //
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,                                      //
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,                            //
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                               //
//                                                                                                                                                                                          //
//                                                                                                                                                             //
// https://github.com/neos-sdi/adfsmfa                                                                                                                                                      //
//                                                                                                                                                                                          //
//******************************************************************************************************************************************************************************************//
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;
using System.Runtime.Versioning;

// Les informations générales relatives à un assembly dépendent de 
// l'ensemble d'attributs suivant. Pour modifier les informations
// associées à un assembly, changez les valeurs de ces attributs.
[assembly: AssemblyTitle("Neos.IdentityServer.MultiFactor.DataTypes")]
[assembly: AssemblyDescription("ADFS MFA Provider")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("neos-sdi")]
[assembly: AssemblyProduct("Microsoft.IdentityServer.MultiFactor.DataTypes")]
[assembly: AssemblyCopyright("Copyright redhook © 2023")]
[assembly: AssemblyTrademark("neos-sdi")]
[assembly: AssemblyCulture("")]


// L'affectation de la valeur false à ComVisible rend les types invisibles dans cet assembly 
// aux composants COM. Si vous devez accéder à un type dans cet assembly à partir de 
// COM, affectez la valeur true à l'attribut ComVisible sur ce type.
[assembly: ComVisible(false)]

// Le GUID suivant est pour l'ID de la typelib si ce projet est exposé à COM
[assembly: Guid("d6235d15-6e4c-406f-8bbe-fa59241bcc13")]

// Les informations de version pour un assembly se composent des quatre valeurs suivantes :
//
//      Version principale
//      Version secondaire 
//      Numéro de build
//      Révision
//
[assembly: AssemblyVersion("3.0.0.0")]
[assembly: AssemblyFileVersion("3.1.2403.1")]
[assembly: AssemblyInformationalVersion("3.0.0.0")]
[assembly: NeutralResourcesLanguage("en")]

// To Generate PubliKey 
//
// 1)
// sn -k FriendAssemblies.snk
//   Or 
// sn -k FriendAssemblies.pfx
//
// 2)
// sn -p FriendAssemblies.snk FriendAssemblies.publickey
//   Or
// sn -p FriendAssemblies.pfx FriendAssemblies.publickey
//
// 3)
// sn -tp FriendAssemblies.publickey 
[assembly: InternalsVisibleTo("Neos.IdentityServer.MultiFactor.NotificationHub, PublicKey=0024000004800000140100000602000000240000525341310008000001000100edf579d3fc38cc51c2b64c998e34fc8b913e3533322064e89c17bc15760af5ea2ea0ba37357112480bbe28f9c11614484f62f8a13fff2a1db2236ced53b1fda85624d68ca4278d33212b21891a32ce24e843373f8352309f9bc59734db2024083079346381189168e7e1a22f2af266eb317ae9b648d99fd4bff28b3ef73b279dd180aa7e99148aaa4eda69d4527f7894486c31567161b5c378c1d88d810fb4ebcb3b2a70b5bba31d6d6a43e894ef2c3916a477e5589d356382177de39067ba77e85dba3c42dc23a973597b67a2bc3d6db409be517a93d04e5743813abfdd60e1d0f7d511df8f6208de8018978f6f15780b9d67eebbf9f5b42456a1035110bae6")]
[assembly: InternalsVisibleTo("Neos.IdentityServer.MultiFactor.Data,   PublicKey=0024000004800000140100000602000000240000525341310008000001000100edf579d3fc38cc51c2b64c998e34fc8b913e3533322064e89c17bc15760af5ea2ea0ba37357112480bbe28f9c11614484f62f8a13fff2a1db2236ced53b1fda85624d68ca4278d33212b21891a32ce24e843373f8352309f9bc59734db2024083079346381189168e7e1a22f2af266eb317ae9b648d99fd4bff28b3ef73b279dd180aa7e99148aaa4eda69d4527f7894486c31567161b5c378c1d88d810fb4ebcb3b2a70b5bba31d6d6a43e894ef2c3916a477e5589d356382177de39067ba77e85dba3c42dc23a973597b67a2bc3d6db409be517a93d04e5743813abfdd60e1d0f7d511df8f6208de8018978f6f15780b9d67eebbf9f5b42456a1035110bae6")]
[assembly: InternalsVisibleTo("Neos.IdentityServer.MultiFactor.Common, PublicKey=0024000004800000140100000602000000240000525341310008000001000100edf579d3fc38cc51c2b64c998e34fc8b913e3533322064e89c17bc15760af5ea2ea0ba37357112480bbe28f9c11614484f62f8a13fff2a1db2236ced53b1fda85624d68ca4278d33212b21891a32ce24e843373f8352309f9bc59734db2024083079346381189168e7e1a22f2af266eb317ae9b648d99fd4bff28b3ef73b279dd180aa7e99148aaa4eda69d4527f7894486c31567161b5c378c1d88d810fb4ebcb3b2a70b5bba31d6d6a43e894ef2c3916a477e5589d356382177de39067ba77e85dba3c42dc23a973597b67a2bc3d6db409be517a93d04e5743813abfdd60e1d0f7d511df8f6208de8018978f6f15780b9d67eebbf9f5b42456a1035110bae6")]
[assembly: InternalsVisibleTo("Neos.IdentityServer.MultiFactor.Notifications, PublicKey=0024000004800000140100000602000000240000525341310008000001000100edf579d3fc38cc51c2b64c998e34fc8b913e3533322064e89c17bc15760af5ea2ea0ba37357112480bbe28f9c11614484f62f8a13fff2a1db2236ced53b1fda85624d68ca4278d33212b21891a32ce24e843373f8352309f9bc59734db2024083079346381189168e7e1a22f2af266eb317ae9b648d99fd4bff28b3ef73b279dd180aa7e99148aaa4eda69d4527f7894486c31567161b5c378c1d88d810fb4ebcb3b2a70b5bba31d6d6a43e894ef2c3916a477e5589d356382177de39067ba77e85dba3c42dc23a973597b67a2bc3d6db409be517a93d04e5743813abfdd60e1d0f7d511df8f6208de8018978f6f15780b9d67eebbf9f5b42456a1035110bae6")]



