FROM node:22-alpine
WORKDIR /app

# install dependencies
COPY package*.json ./
RUN npm install
RUN npm i -g serve

# copy application code 
COPY . .

# build application
RUN npm run build
EXPOSE 3000

# run application
CMD [ "serve", "-s", "dist" ]