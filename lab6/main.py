from datetime import datetime

from flask import Flask, jsonify, request, render_template
from flask_socketio import SocketIO
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config["SECRET_KEY"] = "asdad"
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///database.db'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
db = SQLAlchemy(app)

socketio = SocketIO(app)


# WebSocket event to handle new connections
@socketio.on('connect')
def handle_connect():
    print("A user has connected.")
    # Optional: Send a message to the client on connection
    # send("Welcome to the server!")


# WebSocket event to handle disconnections
@socketio.on('disconnect')
def handle_disconnect():
    print("A user has disconnected.")


@socketio.on('new_ticket')
def handle_new_ticket(data):
    # Extract data from the message
    description = data.get('description')
    priority = data.get('priority')
    comments = data.get('comments', 'Brak')
    # Create new ticket and save to database
    new_ticket = Ticket(
        description=description,
        priority=priority,
        status="Nowe",  # Default status
        comments=comments
    )
    db.session.add(new_ticket)
    db.session.commit()
    print("nowy tickecik" + description)
    socketio.emit('new_ticket', {
        'id': new_ticket.id,
        'description': new_ticket.description,
        'priority': new_ticket.priority,
        'status': new_ticket.status,
        'comments': new_ticket.comments,
        'created_at': new_ticket.created_at.strftime('%Y-%m-%d %H:%M:%S'),
        'updated_at': new_ticket.updated_at.strftime('%Y-%m-%d %H:%M:%S')
    }, include_self=False)


@socketio.on('new_update')
def handle_ticket_update(data):
    ticket_id = data.get('id')
    status = data.get('status')
    comments = data.get('comments')

    ticket = Ticket.query.get(ticket_id)
    print("ticket zupdatowany")
    if ticket:
        print("wchodze w ifa ticket new update")

        ticket.status = status
        ticket.comments = comments
        ticket.updated_at = datetime.now()
        db.session.commit()
        socketio.emit('ticket_update', {
            'id': ticket.id,
            'description': ticket.description,
            'priority': ticket.priority,
            'status': ticket.status,
            'comments': ticket.comments,
            'created_at': ticket.created_at.strftime('%Y-%m-%d %H:%M:%S'),
            'updated_at': ticket.updated_at.strftime('%Y-%m-%d %H:%M:%S')
        }, include_self=False)


# Model bazy danych
class Ticket(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    description = db.Column(db.String(500), nullable=False)
    priority = db.Column(db.Integer, nullable=False)
    status = db.Column(db.String(20), default="Nowe")
    created_at = db.Column(db.DateTime, default=datetime.now)
    updated_at = db.Column(db.DateTime, default=datetime.now)
    comments = db.Column(db.String(500), default="Brak")


# Inicjalizacja bazy danych
with app.app_context():
    db.create_all()


# Strona główna
@app.route('/')
def index():
    return render_template('index.html')


# Strona użytkownika
@app.route('/user')
def user():
    return render_template('user.html')


# Strona administratora
@app.route('/admin')
def admin():
    return render_template('admin.html')


# API do dodawania zgłoszenia (POST)
@app.route('/api/tickets', methods=['POST'])
def add_ticket():
    data = request.get_json()

    # Sprawdzenie, czy wymagane dane są obecne
    if not data.get('description') or not data.get('priority'):
        return jsonify({'error': 'Opis i priorytet są wymagane!'}), 400

    new_ticket = Ticket(
        description=data['description'],
        priority=data['priority'],
        status="Nowe",  # Domyślny status
        comments=data.get('comments', 'Brak')
    )

    return jsonify({
        'id': new_ticket.id,
        'description': new_ticket.description,
        'priority': new_ticket.priority,
        'status': new_ticket.status,
        'comments': new_ticket.comments,
    }), 201


# API do pobierania zgłoszeń
@app.route('/api/tickets', methods=['GET'])
def get_tickets():
    tickets = Ticket.query.all()
    tickets_data = [{
        'id': ticket.id,
        'description': ticket.description,
        'status': ticket.status,
        'priority': ticket.priority,
        'created_at': ticket.created_at.strftime('%Y-%m-%d %H:%M:%S'),
        'updated_at': ticket.updated_at.strftime('%Y-%m-%d %H:%M:%S'),
        'comments': ticket.comments
    } for ticket in tickets]
    return jsonify(tickets_data)


# API do pobierania pojedynczego zgłoszenia
@app.route('/api/tickets/<int:ticket_id>', methods=['GET'])
def get_ticket(ticket_id):
    ticket = Ticket.query.get_or_404(ticket_id)
    return jsonify({
        'id': ticket.id,
        'description': ticket.description,
        'status': ticket.status,
        'priority': ticket.priority,
        'created_at': ticket.created_at.strftime('%Y-%m-%d %H:%M:%S'),
        'updated_at': ticket.updated_at.strftime('%Y-%m-%d %H:%M:%S'),
        'comments': ticket.comments
    })


# API do aktualizacji zgłoszenia
@app.route('/api/tickets/<int:ticket_id>', methods=['PUT'])
def update_ticket(ticket_id):
    ticket = Ticket.query.get_or_404(ticket_id)
    data = request.get_json()

    ticket.status = data['status']
    ticket.comments = data['comments']
    ticket.updated_at = datetime.now()

    db.session.commit()

    return jsonify({'message': 'Zgłoszenie zostało zaktualizowane.'})


if __name__ == "__main__":
    socketio.run(app, debug=True, use_reloader=True, allow_unsafe_werkzeug=True, log_output=True)
