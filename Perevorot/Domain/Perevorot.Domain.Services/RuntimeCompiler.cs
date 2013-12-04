using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using System.Text;
using Winner.Domain.Core.Models;

namespace Winner.Domain.Services
{
   public  class RuntimeCompiler
    {
       public  IRobotBehavior CreateInstance(string code)
       {
           var instanceType = BuildAssembly(code).GetTypes().First();
           return (IRobotBehavior)Activator.CreateInstance(instanceType);
       }
        private Assembly BuildAssembly(string code)
        {
            var parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("Winner.Domain.Core.dll");
            var results = CodeDomProvider.CreateProvider("C#")
                                         .CompileAssemblyFromSource(parameters, code);
            

            if (results.Errors.HasErrors)
            {
                var errors = new StringBuilder("Compiler Errors :\r\n");
                foreach (CompilerError error in results.Errors)
                {
                    errors.AppendFormat("Line {0},{1}\t: {2}\n",
                           error.Line, error.Column, error.ErrorText);
                }
                throw new Exception(errors.ToString());
            }
            return results.CompiledAssembly;
        }
    }
}
