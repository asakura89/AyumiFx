using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asakura89.Utility.Data;
using NUnit.Framework;

namespace Asakura89.Utility.Test
{
    [TestFixture]
    public class QueryStringBuilderTest
    {
        [Test]
        public void SelectAllTest()
        {
            QueryStringBuilder queryBuilder = new QueryStringBuilder();
            queryBuilder.tableName = "WorkBox";
            String actualQuery = queryBuilder.BuildQueryString();

            String expectedQuery = "SELECT * FROM WorkBox";

            Assert.AreEqual(expectedQuery, actualQuery);
        }

        [Test]
        public void SelectTest()
        {
            QueryStringBuilder queryBuilder = new QueryStringBuilder();
            queryBuilder.tableName = "WorkBox";
            queryBuilder.fieldList = new List<String>() { "Title" };
            String actualQuery = queryBuilder.BuildQueryString();

            String expectedQuery = "SELECT Title FROM WorkBox";

            Assert.AreEqual(expectedQuery, actualQuery);
        }

        [Test]
        public void CustomSelectTest()
        {
            QueryStringBuilder queryBuilder = new QueryStringBuilder();
            queryBuilder.defaultQuery = "SELECT Title FROM WorkBox";

            List<Criteria> criteriaParamList = new List<Criteria>();
            Criteria criteriaParam = new Criteria();
            criteriaParam.fieldName = "BoxId";
            criteriaParam.op = CriteriaOperator.EQ;
            criteriaParam.value = "6672EFF5-E3FE-4F78-80B7-0B02CA35A244";
            criteriaParam.type = CriteriaValueType.STRING;
            criteriaParamList.Add(criteriaParam);

            queryBuilder.criteriaList = criteriaParamList;
            String actualQuery = queryBuilder.BuildQueryString();

            String expectedQuery = "SELECT Title FROM WorkBox WHERE BoxId = '6672EFF5-E3FE-4F78-80B7-0B02CA35A244'";

            Assert.AreEqual(expectedQuery, actualQuery);
        }

        [Test]
        public void FullFeaturedSelectTest()
        {
            QueryStringBuilder queryBuilder = new QueryStringBuilder();
            queryBuilder.tableName = "WorkItem wi";
            queryBuilder.fieldList = (new[] {"wi.Title", "wi.Done"}).ToList();

            List<Criteria> criteriaParamList = new List<Criteria>();
            Criteria criteriaParam = new Criteria();
            criteriaParam.fieldName = "wi.Title";
            criteriaParam.op = CriteriaOperator.LIKE;
            criteriaParam.value = "%Punya%";
            criteriaParam.type = CriteriaValueType.STRING;
            criteriaParamList.Add(criteriaParam);
            List<String> sortCriteriaList = (new[] {"wi.Title ASC", "wi.Done DESC"}).ToList();
            List<String> groupCriteriaList = (new[] {"wi.Done", "wi.Title"}).ToList();

            queryBuilder.criteriaList = criteriaParamList;
            queryBuilder.sortCriteriaList = sortCriteriaList;
            queryBuilder.groupCriteriaList = groupCriteriaList;
            String actualQuery = queryBuilder.BuildQueryString();

            String expectedQuery = "SELECT wi.Title, wi.Done FROM WorkItem wi " +
                                   "WHERE wi.Title LIKE '%Punya%' " +
                                   "GROUP BY wi.Done, wi.Title " +
                                   "ORDER BY wi.Title ASC, wi.Done DESC";

            Assert.AreEqual(expectedQuery, actualQuery);
        }
    }
}
