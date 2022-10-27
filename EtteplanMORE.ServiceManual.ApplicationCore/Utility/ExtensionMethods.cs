using MongoDB.Driver;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Utility
{
    internal class ExtensionMethods
    {
        public static DatabaseResult CheckDatabaseResult(ReplaceOneResult result)
        {
            return CheckDatabaseResult(result.ModifiedCount, result.MatchedCount, result.UpsertedId, result.IsAcknowledged);
        }

        public static DatabaseResult CheckDatabaseResult(UpdateResult result)
        {
            return CheckDatabaseResult(result.ModifiedCount, result.MatchedCount, result.UpsertedId, result.IsAcknowledged);
        }

        public static bool DatabaseResultWasPositive(ReplaceOneResult result)
        {
            DatabaseResult databaseResult = CheckDatabaseResult(result.ModifiedCount, result.MatchedCount, result.UpsertedId, result.IsAcknowledged);
            return (databaseResult == DatabaseResult.created) || (databaseResult == DatabaseResult.modified) || (databaseResult == DatabaseResult.foundButNotModified);
        }

        public static bool DatabaseResultWasPositive(UpdateResult result)
        {
            DatabaseResult databaseResult = CheckDatabaseResult(result.ModifiedCount, result.MatchedCount, result.UpsertedId, result.IsAcknowledged);
            return (databaseResult == DatabaseResult.created) || (databaseResult == DatabaseResult.modified) || (databaseResult == DatabaseResult.foundButNotModified);
        }

        private static DatabaseResult CheckDatabaseResult(long? modifiedCount, long? matchedCount, MongoDB.Bson.BsonValue upsertId, bool isAcknowledged)
        {
            if (!isAcknowledged)
            {
                return DatabaseResult.notAcknowleged;
            }

            if (!string.IsNullOrEmpty((string)upsertId))
            {
                return DatabaseResult.created;
            }

            if (matchedCount == 0 && modifiedCount == 0)
            {
                return DatabaseResult.notFound;
            }
            else if (matchedCount == 1 && modifiedCount == 0)
            {
                return DatabaseResult.foundButNotModified;
            }

            else if (matchedCount == 1 && modifiedCount == 1)
            {
                return DatabaseResult.modified;
            }

            return DatabaseResult.unKnown;
        }

        public enum DatabaseResult
        {
            created, modified, foundButNotModified, notFound, notAcknowleged, unKnown
        }
    }
}
