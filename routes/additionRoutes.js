import express from 'express';
import { saveAdditionData, getAdditionData } from '../storage/fileStorage.js';

const router = express.Router();

// GET /api/addition-data 
router.get('/addition-data', async (req, res) => {
  try {
    const data = await getAdditionData();
    
    if (data === null) {
      return res.status(404).json({ 
        data: null,
        message: 'No addition data found' 
      });
    }
    
    res.status(200).json({
      success: true,
      data: data
    });
    
  } catch (error) {
    res.status(500).json({ 
      error: 'Failed to retrieve addition data',
      details: error.message 
    });
  }
});

// PUT /api/addition-data 
router.put('/addition-data', async (req, res) => {
  try {
    const { history, sum } = req.body;
    
    // Validate the data structure
    if (!Array.isArray(history) || typeof sum !== 'number') {
      return res.status(400).json({ 
        error: 'Invalid data format. Expected history array and sum number.' 
      });
    }
    
    // Validate that all history items are integers
    if (!history.every(num => Number.isInteger(num))) {
      return res.status(400).json({
        error: 'All history items must be integers.'
      });
    }
    
    // Validate that sum matches history
    const calculatedSum = history.reduce((acc, num) => acc + num, 0);
    if (calculatedSum !== sum) {
      return res.status(400).json({
        error: 'Sum does not match history calculation.'
      });
    }
    
    const persistedData = { history, sum };
    await saveAdditionData(persistedData);
    
    res.status(200).json({ 
      success: true, 
      message: 'Addition data saved successfully',
      data: persistedData 
    });
    
  } catch (error) {
    res.status(500).json({ 
      error: 'Failed to save addition data',
      details: error.message 
    });
  }
});

export default router;