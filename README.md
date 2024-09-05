# Welcome to Mark Wlodawski's Subreddit fetching application

## Description

### I created this .NET/C# application in order to periodically fetch subreddit posts from r/dadjokes, one of my favorites. Every thirty seconds, I send a GET request to the https://oauth.reddit.com/r/dadjokes/top endpoint to retrieve the top posts, and I do so for three minutes. After retrieving the data, I parse the post data and print out the Title and URL of each of these top posts to the console. I was unable to find a public source of the top users for any subreddit, although there is an api for reddit moderators to use.

## Security

### I stored the necessary token in an Azure Key Vault in order for it to be more secure and inaccessible from the application code.
