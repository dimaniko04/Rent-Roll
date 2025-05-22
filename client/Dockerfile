FROM node:22-alpine
WORKDIR /app

# install dependencies
COPY package*.json ./
RUN npm install

# copy application code 
COPY . .

# run application
EXPOSE 3000
CMD [ "npm", "run", "dev" ]