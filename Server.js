const express = require('express');
const fs = require('fs');

const app = express();
app.use(express.json());

const FILE = 'storage.json';

// Read data
function readData() {
  if (!fs.existsSync(FILE)) return {};
  return JSON.parse(fs.readFileSync(FILE));
}

// Write data
function writeData(data) {
  fs.writeFileSync(FILE, JSON.stringify(data, null, 2));
}

// SET ITEM (store two numbers)
app.post('/storage', (req, res) => {
  const { key, value } = req.body;

  if (!Array.isArray(value) || value.length !== 2) {
    return res.status(400).json({ error: 'Please provide exactly two numbers' });
  }

  const data = readData();
  data[key] = value;
  writeData(data);

  res.status(201).json({ message: 'Numbers stored successfully' });
});

// GET ITEM + ADDITION
app.get('/storage/:key', (req, res) => {
  const data = readData();
  const value = data[req.params.key];

  if (!value) {
    return res.status(404).json({ error: 'Key not found' });
  }

  const sum = value[0] + value[1];

  res.json({
    key: req.params.key,
    numbers: value,
    sum: sum
  });
});

app.listen(3000, () => {
  console.log('Server running on http://localhost:3000');
});