### STAGE 1:BUILD ###
FROM node:16.16.0-slim as Build

# Create a Virtual directory inside the docker image
WORKDIR /app

COPY ./package.json .
COPY ./package-lock.json .
RUN npm i -f

# Copy files from local machine to virtual directory in docker image
COPY . .

# Run command in Virtual directory

RUN npm run ngb-prod

### STAGE 2:RUN ###
FROM nginx:1.18.0-alpine

# Copying compiled code and nginx config to different folder
COPY --from=build /app/dist/project-angular /usr/share/nginx/html
COPY nginx.conf  /etc/nginx/conf.d/default.conf

# Exposing a port, here it means that inside the container
EXPOSE 80

COPY docker-entrypoint.sh .
RUN chmod +x docker-entrypoint.sh
ENTRYPOINT ["./docker-entrypoint.sh"]


