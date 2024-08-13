#!/bin/bash

# Function to display usage information
usage() {
    echo "Usage: $0 [OPTIONS]"
    echo "Options:"
    echo "  -h, --help                     Display this help message"
    echo "  --az-service-bus STRING        Azure Service Bus connection string"
    echo "  --az-app-insights STRING       Application Insights connection string"
    echo "  --rabbit-host HOST             RabbitMQ host"
    echo "  --rabbit-username USERNAME     RabbitMQ username"
    echo "  --rabbit-password PASSWORD     RabbitMQ password"
    echo "  --email-server SERVER          Email server"
    echo "  --email-port PORT              Email port"
    echo "  --email-address ADDRESS        Email address"
    echo "  --email-password PASSWORD      Email password"
    echo "  --feature-app-insights VALUE   Enable or disable AppInsights feature (true/false)"
    echo "  --feature-service-bus VALUE    Enable or disable ServiceBus feature (true/false)"
    echo "  --feature-rabbitmq VALUE       Enable or disable RabbitMQ feature (true/false)"
    echo "  --admin-email EMAIL            Admin email address"
    echo "  --admin-password PASSWORD      Admin password"
    echo "  --client-secret SECRET         Client secret"
    echo "  --client-callback-uri URI      Client callback URI"
    exit 1
}

# Check if no arguments are provided
if [ "$#" -eq 0 ]; then
    usage
fi

# Parse command-line arguments
while [[ $# -gt 0 ]]; do
    key="$1"
    case $key in
        -h|--help)
            usage
            ;;
        --az-service-bus)
            azServiceBus="$2"
            shift
            shift
            ;;
        --az-app-insights)
            azAppInsightsConnectionString="$2"
            shift
            shift
            ;;
        --rabbit-host)
            rabbitHost="$2"
            shift
            shift
            ;;
        --rabbit-username)
            rabbitUsername="$2"
            shift
            shift
            ;;
        --rabbit-password)
            rabbitPassword="$2"
            shift
            shift
            ;;
        --email-server)
            emailServer="$2"
            shift
            shift
            ;;
        --email-port)
            emailPort="$2"
            shift
            shift
            ;;
        --email-address)
            emailAddress="$2"
            shift
            shift
            ;;
        --email-password)
            emailPassword="$2"
            shift
            shift
            ;;
        --feature-app-insights)
            featureAppInsights="$2"
            shift
            shift
            ;;
        --feature-service-bus)
            featureServiceBus="$2"
            shift
            shift
            ;;
        --feature-rabbitmq)
            featureRabbitmq="$2"
            shift
            shift
            ;;
        --admin-email)
            adminEmail="$2"
            shift
            shift
            ;;
        --admin-password)
            adminPassword="$2"
            shift
            shift
            ;;
        --client-secret)
            clientSecret="$2"
            shift
            shift
            ;;
        --client-callback-uri)
            clientCallbackUri="$2"
            shift
            shift
            ;;
        *)
            echo "Invalid option: $1"
            usage
            ;;
    esac
done

# Function to escape special characters for JSON
escape_json() {
    echo "$1" | sed 's/[\/&]/\\&/g; s/"/\\"/g'
}

# Replace placeholder values in the appsettings.json file
if [ ! -z "$azServiceBus" ]; then
    sed -i "s/{azServiceBus}/$(escape_json "$azServiceBus")/g" appsettings.json
fi

if [ ! -z "$azAppInsightsConnectionString" ]; then
    sed -i "s/{azAppInsightsConnectionString}/$(escape_json "$azAppInsightsConnectionString")/g" appsettings.json
fi

if [ ! -z "$rabbitHost" ]; then
    sed -i "s/{rabbitHost}/$(escape_json "$rabbitHost")/g" appsettings.json
fi

if [ ! -z "$rabbitUsername" ]; then
    sed -i "s/{rabbitUsername}/$(escape_json "$rabbitUsername")/g" appsettings.json
fi

if [ ! -z "$rabbitPassword" ]; then
    sed -i "s/{rabbitPassword}/$(escape_json "$rabbitPassword")/g" appsettings.json
fi

if [ ! -z "$emailServer" ]; then
    sed -i "s/{emailServer}/$(escape_json "$emailServer")/g" appsettings.json
fi

if [ ! -z "$emailPort" ]; then
    sed -i "s/{emailPort}/$(escape_json "$emailPort")/g" appsettings.json
fi

if [ ! -z "$emailAddress" ]; then
    sed -i "s/{emailAddress}/$(escape_json "$emailAddress")/g" appsettings.json
fi

if [ ! -z "$emailPassword" ]; then
    sed -i "s/{emailPassword}/$(escape_json "$emailPassword")/g" appsettings.json
fi

if [ ! -z "$adminEmail" ]; then
    sed -i "s/{adminEmail}/$(escape_json "$adminEmail")/g" appsettings.json
fi

if [ ! -z "$adminPassword" ]; then
    sed -i "s/{adminPassword}/$(escape_json "$adminPassword")/g" appsettings.json
fi

if [ ! -z "$clientSecret" ]; then
    sed -i "s/{clientSecret}/$(escape_json "$clientSecret")/g" appsettings.json
fi

if [ ! -z "$clientCallbackUri" ]; then
    sed -i "s/{clientCallbackUri}/$(escape_json "$clientCallbackUri")/g" appsettings.json
fi

# Replace feature management values
if [ ! -z "$featureAppInsights" ]; then
    sed -i "s/\"AppInsights\": false/\"AppInsights\": $featureAppInsights/g" appsettings.json
fi

if [ ! -z "$featureServiceBus" ]; then
    sed -i "s/\"ServiceBus\": false/\"ServiceBus\": $featureServiceBus/g" appsettings.json
fi

if [ ! -z "$featureRabbitmq" ]; then
    sed -i "s/\"RabbitMq\": false/\"RabbitMq\": $featureRabbitmq/g" appsettings.json
fi

echo "Placeholder values replaced in appsettings.json file."