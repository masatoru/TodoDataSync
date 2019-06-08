#!/usr/bin/env bash

GOOGLE_JSON_FILE=$APPCENTER_SOURCE_DIRECTORY/TodoDataSync/TodoDataSync.Android/google-services.json

echo "PATH=" $GOOGLE_JSON_FILE

if [ -e "$GOOGLE_JSON_FILE" ]
then
    echo "Updating Google Json"
    echo "$GOOGLE_JSON" > $GOOGLE_JSON_FILE
    sed -i -e 's/\\"/'\"'/g' $GOOGLE_JSON_FILE

    echo "File content:"
    cat $GOOGLE_JSON_FILE
fi


ANDROID_MANIFEST_FILE=$APPCENTER_SOURCE_DIRECTORY/TodoDataSync/TodoDataSync.Android/Properties/AndroidManifest.xml

echo "PATH=" $ANDROID_MANIFEST_FILE

if [ -e "$ANDROID_MANIFEST_FILE" ]
then
    sed -i '' 's#package="[-A-Za-z0-9:_./]*"#package="'$ANDROID_PACKAGE_NAME'"#' $ANDROID_MANIFEST_FILE
    sed -i '' 's#android:scheme="[-A-Za-z0-9:_./]*"#android:scheme="msal'$APPCENTER_KEY_ANDROID'"#' $ANDROID_MANIFEST_FILE

    echo "File content:"
    cat $ANDROID_MANIFEST_FILE
fi


APP_CONSTANT_FILE=$APPCENTER_SOURCE_DIRECTORY/TodoDataSync/TodoDataSync/Models/AppCenterConfiguration.cs

if [ -e "$APP_CONSTANT_FILE" ]
then
    sed -i '' 's#Android = "[-A-Za-z0-9:_./]*"#Android = "'$APPCENTER_KEY_ANDROID'"#' $APP_CONSTANT_FILE

    echo "File content:"
    cat $APP_CONSTANT_FILE
fi



