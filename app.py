from flask import Flask, request
import mysql.connector
from flask_cors import CORS

app = Flask(__name__)
CORS(app, origins="*")

#Connecting to the database
db = mysql.connector.connect(
    host="localhost",
    user="root",
    database="plannerapp"
)

@app.route("/")
def index():
    return "Planner App API"

@app.route("/insert", methods=["POST"])
def insert():
    #Getting data from the POST request
    data = request.get_json()

    mycursor = db.cursor()

    #Note that password is the hash, not the actual password
    #Precondition - password is hashed already
    insert_query = "INSERT INTO users (firstName, lastName, email, password, verified) VALUES (%s, %s, %s, %s, %b)"
    values = (data["firstName"], data["lastName"], data["email"], data["password"], False)

    mycursor.execute(insert_query, values)
    db.commit() #Do this to save changes

if __name__ == "__main__":
    app.run(debug=True)