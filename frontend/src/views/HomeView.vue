<script>
import { FilterMatchMode } from 'primevue/api'
import { NewsSourceService } from '@/service/NewsSourceService'

export default {
  data() {
    return {
      newsSources: null,
      newsSourceDialog: false,
      deleteNewsSourceDialog: false,
      deleteNewsSourcesDialog: false,
      newsSource: {},
      selectedNewsSources: null,
      filters: {},
      submitted: false
    }
  },
  created() {
    this.initFilters()
  },
  mounted() {
    NewsSourceService.getNewsSources().then((data) => (this.newsSources = data))
  },
  methods: {
    formatCurrency(value) {
      if (value) return value.toLocaleString('en-US', { style: 'currency', currency: 'USD' })
      return
    },
    openNew() {
      this.newsSource = {}
      this.submitted = false
      this.newsSourceDialog = true
    },
    queueRSS() {
      NewsSourceService.queueRSS()
    },
    queueContent() {
      NewsSourceService.queueContent()
    },
    queueNER() {
      NewsSourceService.queueNER()
    },
    queueClear() {
      NewsSourceService.queueClear()
    },
    hideDialog() {
      this.newsSourceDialog = false
      this.submitted = false
    },
    async saveNewsSource() {
      this.submitted = true
      let response
      if (this.newsSource.title.trim()) {
        if (this.newsSource.id) {
          response = await NewsSourceService.putNewsSource(this.newsSource)
        } else {
          response = await NewsSourceService.postNewsSource(this.newsSource)
        }
        if (response) {
          this.$toast.add({
            severity: 'success',
            summary: 'Successful',
            detail: 'NewsSource ' + this.newsSource.id ? 'Updated' : 'Added',
            life: 3000
          })
        } else {
          this.$toast.add({
            severity: 'danger',
            summary: 'Error',
            detail: 'NewsSource Cannot ' + this.newsSource.id ? 'Updated' : 'Added',
            life: 3000
          })
        }
        NewsSourceService.getNewsSources().then((data) => (this.newsSources = data))

        this.newsSourceDialog = false
        this.newsSource = {}
      }
    },
    editNewsSource(newsSource) {
      this.newsSource = { ...newsSource }
      this.newsSourceDialog = true
    },
    confirmDeleteNewsSource(newsSource) {
      this.newsSource = newsSource
      this.deleteNewsSourceDialog = true
    },
    async deleteNewsSource() {
      const response = await NewsSourceService.deleteNewsSource(this.newsSource?.id)
      console.log(response)
      this.deleteNewsSourceDialog = false
      if (response === true) {
        this.newsSources = this.newsSources.filter((val) => val.id !== this.newsSource.id)
        this.newsSource = {}
        this.$toast.add({
          severity: 'success',
          summary: 'Successful',
          detail: 'NewsSource deleted',
          life: 3000
        })
        return
      }
      this.$toast.add({
        severity: 'error',
        summary: 'Error',
        detail: 'NewsSource cannot deleted',
        life: 3000
      })
    },
    findIndexById(id) {
      let index = -1
      for (let i = 0; i < this.newsSources.length; i++) {
        if (this.newsSources[i].id === id) {
          index = i
          break
        }
      }

      return index
    },
    createId() {
      let id = ''
      var chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'
      for (var i = 0; i < 5; i++) {
        id += chars.charAt(Math.floor(Math.random() * chars.length))
      }
      return id
    },
    exportCSV() {
      this.$refs.dt.exportCSV()
    },
    confirmDeleteSelected() {
      this.deleteNewsSourcesDialog = true
    },
    deleteselectedNewsSources() {
      this.newsSources = this.newsSources.filter((val) => !this.selectedNewsSources.includes(val))
      this.deleteNewsSourcesDialog = false
      this.selectedNewsSources = null
      this.$toast.add({
        severity: 'success',
        summary: 'Successful',
        detail: 'NewsSources Deleted',
        life: 3000
      })
    },
    initFilters() {
      this.filters = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS }
      }
    }
  }
}
</script>

<template>
  <div>
    <div class="card">
      <Toolbar class="mb-4">
        <template #start>
          <Button label="New" icon="pi pi-plus" severity="success" class="mr-2" @click="openNew" />
          <Button
            label="RSS Kuyruk Oluştur"
            icon="pi pi-plus"
            severity="success"
            class="mr-2"
            @click="queueRSS"
          />
          <Button
            label="Haber Çekme Kuyruk Oluştur"
            icon="pi pi-plus"
            severity="success"
            class="mr-2"
            @click="queueContent"
          />
          <Button
            label="NER-Sentiment Kuyruk Oluştur"
            icon="pi pi-plus"
            severity="success"
            class="mr-2"
            @click="queueNER"
          />
          <Button
            label="Kuyruk Temizle"
            icon="pi pi-plus"
            severity="success"
            class="mr-2"
            @click="queueClear"
          />
        </template>
      </Toolbar>

      <DataTable
        ref="dt"
        :value="newsSources"
        v-model:selection="selectedNewsSources"
        dataKey="id"
        :paginator="true"
        :rows="10"
        :filters="filters"
        paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
        :rowsPerPageOptions="[5, 10, 25]"
        currentPageReportTemplate="Showing {first} to {last} of {totalRecords} newsSources"
      >
        <template #header>
          <div class="flex flex-wrap gap-2 align-items-center justify-content-between">
            <h4 class="m-0">Manage NewsSources</h4>
            <span class="p-input-icon-left">
              <i class="pi pi-search" />
              <InputText v-model="filters['global'].value" placeholder="Search..." />
            </span>
          </div>
        </template>

        <Column field="title" header="Title" sortable style="min-width: 16rem"></Column>
        <Column field="url" header="URL" sortable style="min-width: 16rem"></Column>
        <Column field="rssUrl" header="RSS URL" sortable style="min-width: 16rem"></Column>
        <Column field="xpath" header="XPath" sortable style="min-width: 16rem"></Column>
        <Column field="isSpa" header="SPA" sortable style="min-width: 8rem">
          <template #body="{ data }">
            <i
              class="pi"
              :class="{
                'pi-check-circle text-green-500': data.isSpa,
                'pi-times-circle text-red-400': !data.isSpa
              }"
            ></i>
          </template>
          <template #filter="{ filterModel, filterCallback }">
            <TriStateCheckbox v-model="filterModel.value" @change="filterCallback()" />
          </template>
        </Column>
        <Column field="isActive" header="Status" sortable style="min-width: 10rem">
          <template #body="{ data }">
            <i
              class="pi"
              :class="{
                'pi-check-circle text-green-500': data.isActive,
                'pi-times-circle text-red-400': !data.isActive
              }"
            ></i>
          </template>
          <template #filter="{ filterModel, filterCallback }">
            <TriStateCheckbox v-model="filterModel.value" @change="filterCallback()" /> </template
        ></Column>

        <Column :exportable="false" style="min-width: 8rem">
          <template #body="slotProps">
            <Button
              icon="pi pi-pencil"
              outlined
              rounded
              class="mr-2"
              @click="editNewsSource(slotProps.data)"
            />
            <Button
              icon="pi pi-trash"
              outlined
              rounded
              severity="danger"
              @click="confirmDeleteNewsSource(slotProps.data)"
            />
          </template>
        </Column>
      </DataTable>
    </div>

    <Dialog
      v-model:visible="newsSourceDialog"
      :style="{ width: '450px' }"
      header="NewsSource Details"
      :modal="true"
      class="p-fluid"
    >
      <div class="field">
        <label for="title">Title</label>
        <InputText
          id="title"
          v-model.trim="newsSource.title"
          required="true"
          autofocus
          :class="{ 'p-invalid': submitted && !newsSource.title }"
        />
        <small class="p-error" v-if="submitted && !newsSource.title">Title is required.</small>
      </div>
      <div class="field">
        <label for="url">Url</label>
        <InputText
          id="url"
          v-model.trim="newsSource.url"
          required="true"
          autofocus
          :class="{ 'p-invalid': submitted && !newsSource.url }"
        />
        <small class="p-error" v-if="submitted && !newsSource.url">URL is required.</small>
      </div>
      <div class="field">
        <label for="rssUrl">RSS URL</label>
        <InputText
          id="rssUrl"
          v-model.trim="newsSource.rssUrl"
          required="true"
          autofocus
          :class="{ 'p-invalid': submitted && !newsSource.rssUrl }"
        />
        <small class="p-error" v-if="submitted && !newsSource.rssUrl">RSS URL is required.</small>
      </div>
      <div class="field">
        <label for="xpath">XPath</label>
        <InputText
          id="xpath"
          v-model.trim="newsSource.xpath"
          required="true"
          autofocus
          :class="{ 'p-invalid': submitted && !newsSource.xpath }"
        />
        <small class="p-error" v-if="submitted && !newsSource.xpath">XPath is required.</small>
      </div>
      <div class="flex">
        <div class="field">
          <label for="isActive">Status</label>
          <br />
          <InputSwitch inputId="isActive" v-model="newsSource.isActive" />
        </div>
        <div style="margin-left: 40px" class="field">
          <label for="isSpa">SPA</label>
          <br />
          <InputSwitch inputId="isSpa" v-model="newsSource.isSpa" />
        </div>
      </div>

      <template #footer>
        <Button label="Cancel" icon="pi pi-times" text @click="hideDialog" />
        <Button label="Save" icon="pi pi-check" text @click="saveNewsSource" />
      </template>
    </Dialog>

    <Dialog
      v-model:visible="deleteNewsSourceDialog"
      :style="{ width: '450px' }"
      header="Confirm"
      :modal="true"
    >
      <div class="confirmation-content">
        <i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
        <span v-if="newsSource"
          >Are you sure you want to delete <b>{{ newsSource.title }}</b
          >?</span
        >
      </div>
      <template #footer>
        <Button label="No" icon="pi pi-times" text @click="deleteNewsSourceDialog = false" />
        <Button label="Yes" icon="pi pi-check" text @click="deleteNewsSource" />
      </template>
    </Dialog>

    <Dialog
      v-model:visible="deleteNewsSourcesDialog"
      :style="{ width: '450px' }"
      header="Confirm"
      :modal="true"
    >
      <div class="confirmation-content">
        <i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
        <span v-if="newsSource">Are you sure you want to delete the selected newsSources?</span>
      </div>
      <template #footer>
        <Button label="No" icon="pi pi-times" text @click="deleteNewsSourcesDialog = false" />
        <Button label="Yes" icon="pi pi-check" text @click="deleteselectedNewsSources" />
      </template>
    </Dialog>
  </div>
</template>
