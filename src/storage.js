import fs from 'fs';
import path from 'path';

const DB_PATH = path.join(process.cwd(), 'data', 'storage.json');

function ensureFile() {
  const dir = path.dirname(DB_PATH);
  if (!fs.existsSync(dir)) fs.mkdirSync(dir, { recursive: true });
  if (!fs.existsSync(DB_PATH)) fs.writeFileSync(DB_PATH, JSON.stringify({}), 'utf8');
}

export function readAll() {
  ensureFile();
  const raw = fs.readFileSync(DB_PATH, 'utf8');
  return raw ? JSON.parse(raw) : {};
}

export function writeAll(obj) {
  ensureFile();
  fs.writeFileSync(DB_PATH, JSON.stringify(obj, null, 2), 'utf8');
}

export function getItem(key) {
  const db = readAll();
  return Object.prototype.hasOwnProperty.call(db, key) ? db[key] : undefined;
}

export function setItem(key, value) {
  const db = readAll();
  const existed = Object.prototype.hasOwnProperty.call(db, key);
  db[key] = value;
  writeAll(db);
  return { existed, value };
}
