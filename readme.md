# Jira WorkLog uploader
>> *quick & dirty - but works ;)*

For those who need to log time against JIRA, and don't use Tempo and it's Tracker feature, possibly the easiest way is to log (write) the time to an Excecl sheet first, then upload it to JIRA.

A template Excel and a sort usage document is found inside the **docs** directory.

This aplication allows to use multiple JIRA servers to log agains. In my case I had to create a cummulative log and a detailed log too (each into a separate JIRA server).

**Excel sample & application screenshot:**
![excel](/docs/excel.png "excel")
![app](/docs/application.png "app")

**Used NuGets/technologies**:
- NPOI to work with Excel files
- JSON for storing settings
- PropertyGrid to manage settings on the UI

**Known limitations:**
- at the time when this application was created, it was not possible (or haven't googled hard enough) to determine **JIRA server time-zone settings**. So worklogs to be logged with correct time, this information had to be provided **manually**, via settings.

**Versions:**
- 1.0 - somewhere around summer of 2016

