using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystemManagement
{
    /// <summary>
    /// 
    /// This search class is instantiated when a user requests a search on the current fleet.
    /// This search class is capable of comprehending advanced search query.
    /// The search class comprises of several sequential methods to break down what the user 
    /// query is, then apply the query accordingly to the main search method which is 
    /// called 'searchoutput()'.
    /// More specifically, a received free text query is broken down to two lists: attributes and
    /// operator - where attributes contain the description of the vehicles, whereas operators contain
    /// the conjunctions (either OR/AND). Firstly, 'eachattributesearch()' method is used to yield 
    /// search result for individual attributes. This result is then passed through the main search method 
    /// which then filters the data out according to the operator sequence. 
    /// The 'searchoutput' function uses recursive method where the size of the input reduces as each 
    /// iteration is performed, until eventually it will input a list of size 1. This input result is then 
    /// considered as a final result and therefore returned. 
    /// 
    /// Author: Myungjun Choi 
    /// 
    /// </summary>

    public class Search
    {

        //List<List<string>> fleetAttributes;  // will be saving attributes of all vehicles in the fleet here for searching

        Fleet fleet = new Fleet(); // instantiate the fleet class 

        /// <summary>
        /// Constructor for search class. It adds attributeslist of all vehicles to FleetAttributes 
        /// </summary>
        public Search()
        {
            fleet = new Fleet();
            /*fleetAttributes = new List<List<string>>();
            for (int k = 0; k < fleet.GetFleet().Count; k++)
            {
                List<string> currentCarAttributes = fleet.GetFleet()[k].GetAttributeList().ConvertAll(x => x.ToLower());
                fleetAttributes.Add(currentCarAttributes);
            } // end for*/

        } // end method

        /// <summary>
        /// get the list of query attributes from query string input
        /// </summary>
        /// <param name="query"> query string input customer types into the program </param>
        /// <returns> updated queryList </returns>
        public List<string> getQueryAttributes(string query)
        {
            List<string> queryList = new List<string>();  // stores a list of query keyword attributes
            string[] delimiter = { " and ", " or " }; // sets the two words as keywords for splitting
            // split the query by delimiter keywords and store it as a list
            queryList = query.Split(delimiter, System.StringSplitOptions.RemoveEmptyEntries).ToList();

            return queryList;
        }// end method

        /// <summary>
        /// get the lsit of operater from query string input
        /// </summary>
        /// <param name="query">query string input customer types into the program </param>
        /// <returns></returns>
        public List<string> getOperaterList(string query)
        {
            List<string> operaterList = new List<string>();
            // split the strings with comma and only keep the keyword 'OR' or 'AND'
            operaterList = query.Split(' ').Where(s => (s == "and" || s == "or")).ToList();

            return operaterList;
        } // end method

        /// <summary>
        /// searches the vehicles using a single attribute keyword
        /// </summary>
        /// <param name="attribute"> attribute keyword in string format </param>
        /// <returns></returns>
        public List<Vehicle> singleAttributeSearch(string attribute, int min = 0, int max = 9999999)
        {
            List<Vehicle> singleResult = new List<Vehicle>(); // stores the search result

            // iterate through fleet's attributelist and see if it contains the requested attribute
            for (int i = 0; i < fleet.GetFleet().Count; i++)
            {
                if (fleet.GetFleet()[i].GetAttributeList().ConvertAll(x => x.ToLower()).Contains(attribute)
                    && (min <= fleet.GetFleet()[i].DailyRate && fleet.GetFleet()[i].DailyRate <= max))
                {
                    singleResult.Add(fleet.GetFleet()[i]);
                } // end if 
            } // end for
            return singleResult;
        } // end method

        /// <summary>
        /// the method does singleAttributeSearch for each querylist attributes
        /// and store each results in a list of list. 
        /// This is used for final search when doing multisearch 
        /// </summary>
        /// <param name="queryList"> list of query keywords </param>
        /// <returns></returns>
        public List<List<Vehicle>> eachattributesearch(List<string> queryList, int min = 0, int max = 9999999)
        {

            List<List<Vehicle>> eachresult = new List<List<Vehicle>>();
            // iterate through each keyword and do singleAttributeSearch on that keyword
            for (int i = 0; i < queryList.Count; i++)
            {
                eachresult.Add(singleAttributeSearch(queryList[i], min, max));
            } // end for
            return eachresult;
        } // end method

        /// <summary>
        /// this method does multi-keyword search using a recursive method.
        /// The method recursively reduces the input list until there are only 
        /// 1 element left. This element will be the final result of the multi-
        /// keyword search.
        /// </summary>
        /// <param name="eachresult"> list of eachattributeSearch </param>
        /// <returns></returns>
        public List<Vehicle> searchoutput(List<List<Vehicle>> eachresult, List<string> operaterList)
        {

            List<Vehicle> current_result = new List<Vehicle>();
            // if the input list only has 1 element, then return that element
            // as it is a final result
            if (eachresult.Count == 1)
            {
                return eachresult[0];
            }
            else if (eachresult.Count == 0)
            {
                return current_result;

            }
            else
            {
                // if the operator between first and second keyword search 
                // result is 'OR', then perform 'OR ' operation. If 'AND',
                // perform 'AND' action. Keywords for such operations are 
                // 'Distinct' and 'Intersect' respectively
                if (operaterList[0] == "or")
                {
                    current_result.AddRange(eachresult[0]);
                    current_result.AddRange(eachresult[1]);
                    current_result = current_result.Distinct().ToList();
                }
                else if (operaterList[0] == "and")
                {
                    current_result.AddRange(eachresult[0]);
                    current_result = current_result.Intersect(eachresult[1]).ToList();
                } // end if 

                // once operation completed, get rid of the first and second 
                // element of the input as we no longer need it. Instead, replace 
                // it with the operation result.
                eachresult.RemoveAt(0);
                eachresult.RemoveAt(0);
                operaterList.RemoveAt(0);   // also remove the used operator 
                eachresult.Insert(0, current_result);
                return searchoutput(eachresult, operaterList); // recurisvely return the reduced version of the method 
            } // end if 
        } // end method
    } // end search class
}
