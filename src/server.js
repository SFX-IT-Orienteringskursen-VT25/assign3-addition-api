import express from 'express';
import cors from 'cors';
import { getItem, setItem } from './storage.js';

const app = express();
app.use(cors());
app.use(express.json()); // parse JSON bodies

// GET /storage/:key  -> like localStorage.getItem(key)
app.get('/storage/:key', (req, res) => {
  const { key } = req.params;
  const value = getItem(key);
  if (value === undefined) {
    return res.status(404).json({ error: 'Not Found' });
  }
  return res.status(200).json({ key, value });
});

// PUT /storage/:key  -> like localStorage.setItem(key, value)
app.put('/storage/:key', (req, res) => {
  const { key } = req.params;
  const { value } = req.body ?? {};

  if (value === undefined) {
    return res.status(400).json({ error: 'value is required' });
  }

  const { existed, value: saved } = setItem(key, value);
  return res.status(existed ? 200 : 201).json({
    key,
    value: saved,
    created: !existed
  });
});

// Start server
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`API running at http://localhost:${PORT}`);
});
