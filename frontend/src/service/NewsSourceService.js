import axios from 'axios'
/* eslint-disable */
export const NewsSourceService = {
  async getNewsSources() {
    const data = await axios.get(`${process.env['CONNECTIONSTRINGS__BACKEND']}/NewsSource`)
    return data.data
  },
  async postNewsSource(newsSource) {
    const data = await axios.post(
      `${process.env['CONNECTIONSTRINGS__BACKEND']}/NewsSource`,
      newsSource
    )
    return data.data
  },
  async putNewsSource(newsSource) {
    const data = await axios.patch(
      `${process.env['CONNECTIONSTRINGS__BACKEND']}/NewsSource`,
      newsSource
    )
    return data.data
  },
  async getSentiments() {
    const data = await axios.get(`${process.env['CONNECTIONSTRINGS__BACKEND']}/News/Sentiments`)
    return data.data
  },
  async deleteNewsSource(id) {
    const data = await axios.delete(`${process.env['CONNECTIONSTRINGS__BACKEND']}/NewsSource/${id}`)
    return data.data
  },
  async queueRSS() {
    return await axios.get(`${process.env['CONNECTIONSTRINGS__BACKEND']}/News/AggregatorSource`)
  },
  async queueContent() {
    return await axios.get(`${process.env['CONNECTIONSTRINGS__BACKEND']}/News/AggregatorNews`)
  },
  async queueNER() {
    return await axios.get(`${process.env['CONNECTIONSTRINGS__BACKEND']}/News/PredictionQueue`)
  },
  async queueClear() {
    return await axios.get(`${process.env['CONNECTIONSTRINGS__BACKEND']}/News/ClearQueue`)
  }
}
