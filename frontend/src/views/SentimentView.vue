<script>
import { NewsSourceService } from '@/service/NewsSourceService'
import * as signalR from '@microsoft/signalr'

export default {
  data: () => {
    return {
      sentiments: []
    }
  },
  mounted() {
    const connection = new signalR.HubConnectionBuilder()
      /*eslint-disable */
      .withUrl(`${process.env['CONNECTIONSTRINGS__BACKEND']}/hub`, {
        skipNegotiation: true,
        transport: 1
      })
      .configureLogging(signalR.LogLevel.Information)
      .build()
    const that = this
    async function start() {
      try {
        await connection.start()
        console.log('SignalR Connected.')
        connection.on('Sentiment', (x) => {
          that.sentiments = [x, ...that.sentiments]
        })
      } catch (err) {
        console.log(err)
        setTimeout(start, 5000)
      }
    }
    start()

    NewsSourceService.getSentiments().then((x) => (this.sentiments = x))
  }
}
</script>

<template>
  <DataTable ref="dt" :value="sentiments">
    <Column field="news.title" header="Title" sortable style="min-width: 16rem"></Column>
    <Column field="type" header="Type" sortable style="min-width: 16rem"></Column>
    <Column field="score" header="Score" sortable style="min-width: 16rem"></Column>
    <Column field="news.url" header="URL" sortable style="min-width: 16rem"></Column>
  </DataTable>
</template>

<style>
@media (min-width: 1024px) {
  .about {
    min-height: 100vh;
    display: flex;
    align-items: center;
  }
}
</style>
