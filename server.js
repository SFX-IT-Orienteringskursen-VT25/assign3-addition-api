const express = require('express');
const fs = require('fs');
const path = require('path');

const app = express();
const PORT = 3000;
const DATA_FILE = path.join(__dirname, 'numbers.json');

app.use(express.json());

// ─── helpers ─────────────────────────────────────────────────────────────────

function readNumbers() {
    try {
        return JSON.parse(fs.readFileSync(DATA_FILE, 'utf-8'));
    } catch {
        return [];
    }
}

function writeNumbers(numbers) {
    fs.writeFileSync(DATA_FILE, JSON.stringify(numbers));
}

function calcSum(numbers) {
    return numbers.reduce((acc, n) => acc + n, 0);
}

// ─── endpoints ───────────────────────────────────────────────────────────────

// Replaces: localStorage.getItem('enteredNumbers')
// Returns the full list of stored numbers + their sum.
app.get('/numbers', (req, res) => {
    const numbers = readNumbers();
    res.status(200).json({ numbers, sum: calcSum(numbers) });
});

// Replaces: localStorage.setItem('enteredNumbers', [...])
// Body: { "number": 42 }
// Adds the number to the persisted list and returns the updated state.
app.post('/numbers', (req, res) => {
    const { number } = req.body;

    if (typeof number !== 'number' || !Number.isInteger(number)) {
        return res.status(400).json({ error: 'Body must contain an integer field "number".' });
    }

    const numbers = readNumbers();
    numbers.push(number);
    writeNumbers(numbers);

    res.status(201).json({ numbers, sum: calcSum(numbers) });
});

// ─────────────────────────────────────────────────────────────────────────────

app.listen(PORT, () => {
    console.log(`Server running on http://localhost:${PORT}`);
});