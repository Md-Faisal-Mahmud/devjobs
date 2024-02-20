# devjobs project

Disclaimer:

1.  If JobTitle, Company.Name, PublishedOn all together insert null in DB. No jobpost'll insert again.
    If so, fixed imediately.
2.  In appsettings.json, if you donot provide PageOneLink, it'll parse first 10 page by default.
    In appsettings.json, AllPostLink of IT/Telecommunication is mandatory.
3.  To maintain compatibility with Serilog.Sinks.Email-2.4.0,
    it's crucial not to update the version of MailKit in DevJobs.DataCollectorService to a version higher than 2.6.0. Updating to a version beyond this may result in compatibility issues.
4.  You have to provide the windows service name in DevJobs.Api -> appsettings.json to show running status
