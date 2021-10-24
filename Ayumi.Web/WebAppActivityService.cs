using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Arvy;
using AutoMapper;
using Databossy;
using WebApp.Model;

namespace WebApp {
    public class WebAppActivityService : IActivityService {
        readonly IDbContextFactory factory;
        readonly IMapper mapper;

        public WebAppActivityService(IDbContextFactory factory) {
            this.factory = factory;
            var config = new MapperConfiguration(conf =>
                conf.CreateMap<m_Activity, WebAppActivity>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ActivityId))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ActivityName))
            );
            mapper = config.CreateMapper();
        }

        public IList<WebAppActivity> GetAllFromAssembly() =>
            Assembly
                .GetCallingAssembly()
                .GetTypes()
                .Where(type => type.Name.ToLowerInvariant().Contains("controller"))
                .SelectMany(type => type
                    .GetMethods()
                    .Where(m => m
                        .GetCustomAttributes(typeof(WebAppAuthorizationAttribute), false)
                        .Any()))
                .Select(m => new WebAppActivity {
                    Id = m.DeclaringType.Name.Replace("Controller", String.Empty) + m.Name,
                    Name = m.Name,
                    Category = m.DeclaringType.Name.Replace("Controller", String.Empty),
                    Description = GetDesc(m.Name)
                })
                .OrderBy(name => name.Category)
                .ToList();

        public IList<WebAppActivity> GetAll(String tenant) {
            IList<m_Activity> activities;
            using (IDatabase db = factory.Context)
                activities = db.Query<m_Activity>(this.LoadEmbeddedEqx("GetAll")).ToList();

            return mapper.Map<IList<m_Activity>, IList<WebAppActivity>>(activities);
        }

        public WebAppActivity Get(String tenant, String id) {
            m_Activity activity;
            using (IDatabase db = factory.Context)
                activity = db.QuerySingle<m_Activity>(this.LoadEmbeddedEqx("Get"), id);

            return mapper.Map<m_Activity, WebAppActivity>(activity);
        }

        public Boolean Find(String tenant, String id) {
            using (IDatabase db = factory.Context)
                return db.QueryScalar<Boolean>(this.LoadEmbeddedEqx("Find"), id);
        }

        public void Add(String tenant, WebAppActivity data) {
            m_Activity activity = mapper.Map<WebAppActivity, m_Activity>(data);
            using (IDatabase db = factory.Context) {
                String result = db.NQueryScalar<String>(this.LoadEmbeddedEqx("Add"), activity);
                result.AsActionResponseViewModel();
            }
        }

        public void Update(String tenant, String id, WebAppActivity data) {
            m_Activity activity = mapper.Map<WebAppActivity, m_Activity>(data);
            using (IDatabase db = factory.Context) {
                String result = db.NQueryScalar<String>(this.LoadEmbeddedEqx("Update"), activity);
                result.AsActionResponseViewModel();
            }
        }

        public void Remove(String tenant, String id) {
            using (IDatabase db = factory.Context) {
                String result = db.NQueryScalar<String>(this.LoadEmbeddedEqx("Remove"), new {Ids = id});
                result.AsActionResponseViewModel();
            }
        }

        String GetDesc(String name) {
            switch (name) {
                case "Index":
                    return "View data";
                case "New":
                    return "Create new data";
                case "TryCreate":
                    return "Save new data";
                case "Update":
                    return "Update existing data";
                case "TryUpdate":
                    return "Save updated data";
                case "TryDelete":
                    return "Delete data";
                default:
                    return name.Replace("Try", String.Empty);
            }
        }
    }
}