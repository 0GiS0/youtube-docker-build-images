FROM nginx:latest

WORKDIR /usr/share/nginx/html

COPY ./site .

EXPOSE 80