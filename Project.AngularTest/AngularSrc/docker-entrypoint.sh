#!/bin/sh

# envsubst < /usr/share/nginx/html/assets/json/HostConfig.default.json > /usr/share/nginx/html/assets/json/HostConfig.json

exec nginx -g 'daemon off;'
