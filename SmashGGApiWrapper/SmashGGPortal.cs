using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace SmashGGApiWrapper
{
    public sealed class SmashGGPortal
    {


        private readonly RestClient client;

        public string tournament { get; private set; }
        public SmashGGPortal(string tournament)
        {
            client = new RestClient(@"https://api.smash.gg/");
            this.tournament = tournament;
        }

        private void throwOnError(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(response.ToString());
            }
        }
        public Tournament GetTournament()
        {
            var request = new RestRequest(string.Format("tournament/{0}", tournament), Method.GET);
            var response = client.Execute<RootObject>(request);
            throwOnError(response);
            return response.Data.entities.tournament;
        }
        public IEnumerable<Event> GetEvents()
        {
            var request = new RestRequest(string.Format("tournament/{0}?expand[]=event", tournament), Method.GET);
            var response = client.Execute<RootObject>(request);
            throwOnError(response);
            return response.Data.entities.@event;
        }
        public Group GetGroup(int groupID)
        {
            var request = new RestRequest(string.Format("phase_group/{0}", groupID), Method.GET);
            var response = client.Execute<RootObject>(request);
            throwOnError(response);
            return response.Data.entities.groups.First();
        }
        public IEnumerable<Group> GetGroups(int eventID)
        {
            var request = new RestRequest(string.Format("event/{0}?expand[]=groups", eventID), Method.GET);
            var response = client.Execute<RootObject>(request);
            throwOnError(response);
            return response.Data.entities.groups;
        }
        public IEnumerable<Group> GetGroupsFromPhase(int phaseID)
        {
            var request = new RestRequest(string.Format("phase/{0}?expand[]=groups", phaseID), Method.GET);
            var response = client.Execute<RootObject>(request);
            throwOnError(response);
            return response.Data.entities.groups;
        }
        public IEnumerable<Phase> GetPhases(int eventID)
        {
            var request = new RestRequest(string.Format("event/{0}?expand[]=phase", eventID), Method.GET);
            var response = client.Execute<RootObject>(request);
            throwOnError(response);
            return response.Data.entities.phase;
        }
        public Phase GetPhase(int phaseID)
        {
            var request = new RestRequest(string.Format("phase/{0}", phaseID), Method.GET);
            var response = client.Execute<RootObject>(request);
            throwOnError(response);
            return response.Data.entities.phase.First();
        }
        public IEnumerable<Set> GetMatches(int groupID)
        {
            var request = new RestRequest(string.Format("phase_group/{0}?expand[]=sets&expand[]=seeds", groupID), Method.GET);
            var response = client.Execute<RootObject>(request);
            throwOnError(response);
            return response.Data.entities.sets;
        }
        public IEnumerable<Entrant> GetEntrants(int groupID)
        {
            var request = new RestRequest(string.Format("phase_group/{0}?expand[]=entrants", groupID), Method.GET);
            var response = client.Execute<RootObject>(request);
            throwOnError(response);
            return response.Data.entities.entrants;
        }
        //public Tournament ShowTournament(String tournamentId)
        //{
        //    if (!string.IsNullOrEmpty(Subdomain)) { tournamentId = Subdomain + "-" + tournamentId; }
        //    var request = new RestRequest(string.Format("tournaments/{0}.xml", tournamentId), Method.GET);
        //    request.AddParameter("api_key", ApiKey);

        //    var response = client.Execute<Tournament>(request);
        //    throwOnError(response);

        //    return response.Data;
        //}

        //public IEnumerable<Match> GetMatches(int tournamentId)
        //{
        //    var request = new RestRequest(string.Format("tournaments/{0}/matches.xml", tournamentId), Method.GET);
        //    request.AddParameter("api_key", ApiKey);

        //    var response = client.Execute<List<Match>>(request);
        //    throwOnError(response);

        //    return response.Data;
        //}

        //public IEnumerable<Participant> GetParticipants(int tournamentId)
        //{
        //    var request = new RestRequest(string.Format("tournaments/{0}/participants.xml", tournamentId), Method.GET);
        //    request.AddParameter("api_key", ApiKey);

        //    var response = client.Execute<List<Participant>>(request);
        //    throwOnError(response);

        //    return response.Data;
        //}

        //public void SetParticipantMisc(int tournamentId, int participantId, string misc)
        //{
        //    var request = new RestRequest(string.Format("tournaments/{0}/participants/{1}.xml", tournamentId, participantId), Method.PUT);
        //    request.AddParameter("api_key", ApiKey);
        //    request.AddParameter("participant[misc]", misc);

        //    var response = client.Execute(request);
        //    throwOnError(response);
        //}
    }
}

