const express = require('express');
const cors = require('cors');

const app = express();
const PORT = 3000;

// Middleware
app.use(cors());
app.use(express.json());

// In-memory storage
let numbers = [];

// GET /numbers — return all stored numbers
app.get('/numbers', (req, res) => {
    res.status(200).json(numbers);
});

// POST /numbers — add a new number
app.post('/numbers', (req, res) => {
    const { number } = req.body;

    if (typeof number !== 'number') {
        return res.status(400).json({ error: 'Invalid number' });
    }

    numbers.push(number);
    res.status(201).json(numbers); // Return updated list
});

// Start server
app.listen(PORT, () => {
    console.log(`Server running on http://localhost:${PORT}`);
});