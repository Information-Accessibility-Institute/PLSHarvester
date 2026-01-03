namespace PLSHarvester

open System
open System.IO

open HtmlExtractor
open KanjiExtractor
open DictionaryBuilder
open PlsWriter
open PlsCleaner

module Program =

    /// Print usage
    let private printUsage () =
        printfn "Usage:  plsharvester INPUT_PATH  [--dict external.tsv]  [--out out.xml]"
        printfn ""
        printfn "INPUT_PATH : HTML, EPUB, or directory"
        printfn "--dict     : Optional external dictionary (PLS/TXT/TSV)"
        printfn "--out      : Output PLS file (default: harvested.pls)"
        printfn ""

    /// Load external dictionary (if given)
    let private loadExternalDictionary (opt: string option) =
        match opt with
        | None -> Set.empty
        | Some path ->
            if File.Exists(path) then
                DictionaryBuilder.loadDictionary path
            else
                failwith $"Dictionary file not found: {path}"

    /// Safe extension helper: Path.GetExtension may return null
    let private getExt (p: string) : string =
        match Path.GetExtension(p) with
        | null -> String.Empty
        | s -> s.ToLowerInvariant()

    /// Recursively collect HTML files or single HTML/EPUB
    let private collectTargets (path: string) : string list =
        if File.Exists(path) then
            match getExt path with
            | ".html" | ".htm" -> [ path ]
            | ".xhtml"         -> [ path ]
            | ".epub"          -> [ path ] // For EPUB, return the path here (extraction is handled in extractTextFromFile)
            | _ ->
                failwith $"Unsupported file: {path}"
        elif Directory.Exists(path) then
            Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
            |> Array.filter (fun f ->
                let ext = getExt f
                ext = ".html" || ext = ".htm" || ext = ".xhtml" || ext = ".epub"
            )
            |> Array.toList
        else
            failwith $"Path not found: {path}"

    /// Extract text from HTML or EPUB
    let private extractTextFromFile (file: string) =
        match getExt file with
        | ".epub" ->
            // For EPUB, use EpubExtractor.extractTextFromEpub to extract internal HTML files and convert them to text
            try
                EpubExtractor.extractTextFromEpub file HtmlExtractor.extractText
            with e ->
                printfn "Error reading %s: %s" file e.Message
                ""
        | _ ->
            try
                HtmlExtractor.loadHtmlFile file
            with e ->
                printfn "Error reading %s: %s" file e.Message
                ""

    [<EntryPoint>]
    let main argv =

        if argv.Length = 0 then
            printUsage ()
            0
        else
            try
                // Parse args
                let inputPath = argv[0]

                let mutable externalDictPath : string option = None
                let mutable outPath : string = "harvested.pls"

                let rec parse i =
                    if i < argv.Length then
                        match argv[i] with
                        | "--dict" ->
                            externalDictPath <- Some argv[i+1]
                            parse (i+2)
                        | "--out" ->
                            outPath <- argv[i+1]
                            parse (i+2)
                        | _ ->
                            parse (i+1)
                parse 1

                // Load external dict
                let externalDict = loadExternalDictionary externalDictPath

                // Collect files
                let targets = collectTargets inputPath

                printfn "Found %d files" targets.Length

                // Extract and aggregate text
                let allText =
                    targets
                    |> List.map extractTextFromFile
                    |> String.concat "\n"

                // Extract Kanji candidates
                let kanjiFreq = KanjiExtractor.extractKanjiCandidates allText

                printfn "Extracted %d unique Kanji sequences" kanjiFreq.Count

                // Filter by external dictionary (remove words present in external dict)
                let filtered =
                    kanjiFreq
                    |> DictionaryBuilder.filterByExternalDict externalDict

                printfn "Remaining %d entries after dictionary filtering" filtered.Count

                // Clean the frequency map (remove low-frequency / short items, optionally those in external dict)
                let existingOpt =
                    if Set.isEmpty externalDict then None else Some externalDict

                // minimum frequency threshold (adjust as needed)
                let minFrequency = 2

                let cleaned =
                    filtered
                    |> PlsCleaner.clean minFrequency existingOpt

                printfn "Remaining %d entries after cleaning" cleaned.Count

                // Write PLS directly using PlsWriter
                PlsWriter.writePls outPath cleaned

                0

            with
            | ex ->
                printfn "Error: %s" ex.Message
                -1
