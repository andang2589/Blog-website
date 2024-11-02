using BlogWebsite.Service.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using M = BlogWebsite.Data.Models;

namespace BlogWebsite.Service.Permission
{
    public class PermissionSeeder
    {
        private readonly ICommonService<M.Permission> _permisisonCmSv;
        public PermissionSeeder(ICommonService<M.Permission> permissionCmSv)
        {
            _permisisonCmSv = permissionCmSv;
        }
        public async Task SeedPermissionAsync()
        {
            var controllersAssembly = Assembly.Load("BlogWebsite.WebApi");

            var controllers = controllersAssembly.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type));

            //var controllers = Assembly.GetExecutingAssembly().GetTypes()
            //    .Where(type => typeof(ControllerBase).IsAssignableFrom(type));

            foreach (var controller in controllers)
            {
                var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(m => m.IsPublic && !m.IsDefined(typeof(NonActionAttribute)));

                foreach (var action in actions)
                {
                    string controllerName = controller.Name.Replace("Controller", "");
                    string actionName = action.Name;

                    //Kiem tra neu permission chua ton tai thi them vao
                    if (!_permisisonCmSv.TableT().Any(p => p.Controller == controllerName && p.Action == actionName))
                    {
                        var permission = new M.Permission
                        {
                            Controller = controllerName,
                            Action = actionName,
                            Name = $"{controllerName}_{actionName}"
                        };
                        await _permisisonCmSv.AddAs(permission);
                    }
                }
                    
            }
        }
    }
}
