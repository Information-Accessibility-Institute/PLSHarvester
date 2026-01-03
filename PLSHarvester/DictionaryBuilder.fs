namespace PLSHarvester

open System
open System.IO
open System.Xml.Linq

/// Build a minimal PLS dictionary (Phase 1):
///   - Only <grapheme> is filled.
///   - No pronunciation (<phoneme>) is included.
///   - Each grapheme entry is unique.
module DictionaryBuilder =

    /// Convert a list of graphemes (kanji sequences) to a PLS XML document (XDocument)
    let buildPLSDocument (graphemes: seq<string>) : XDocument =

        // PLS namespace
        let plsNS = XNamespace.Get("http://www.w3.org/2005/01/pronunciation-lexicon")

        // Remove duplicates & sort
        let uniq =
            graphemes
            |> Seq.distinct
            |> Seq.sort

        // Create <lexeme> entries
        let lexemes =
            uniq
            |> Seq.map (fun g ->
                XElement(
                    plsNS + "lexeme",
                    XElement(plsNS + "grapheme", g)
                    // Phase 1: no <phoneme>
                ))

        // Build the document
        let doc =
            XDocument(
                XElement(
                    plsNS + "lexicon",
                    XAttribute(XNamespace.Xmlns + "pls", plsNS.NamespaceName),
                    XAttribute("version", "1.0"),
                    XAttribute("alphabet", "x-unknown"),
                    lexemes
                )
            )
        doc

    /// Save the PLS document to a file
    let saveToFile (doc: XDocument) (path: string) =
        doc.Save(path)

    /// Load an external dictionary file and return a set of graphemes.
    /// Supported formats: PLS/XML (extracts <grapheme>), TSV/TXT (first column), or plain line list.
    let loadDictionary (path: string) : Set<string> =
        // Path.GetExtension may return null; check for null and fallback to an empty string
        let ext =
            match Path.GetExtension(path) with
            | null -> String.Empty
            | s -> s.ToLowerInvariant() 
        match ext with
        | ".pls" | ".xml" ->
            let doc = XDocument.Load(path)
            // Try to respect the PLS namespace if present, otherwise fall back to any "grapheme" element
            let plsNS = XNamespace.Get("http://www.w3.org/2005/01/pronunciation-lexicon")
            let elems =
                seq {
                    yield! doc.Descendants(plsNS + "grapheme")
                    yield! doc.Descendants(XName.Get("grapheme"))
                }
            elems
            |> Seq.map (fun e -> e.Value.Trim())
            |> Seq.filter (String.IsNullOrWhiteSpace >> not)
            |> Set.ofSeq
        | ".tsv" | ".txt" ->
            File.ReadAllLines(path)
            |> Seq.map (fun line ->
                let cols = line.Split('\t')
                if cols.Length > 0 then cols.[0].Trim() else String.Empty)
            |> Seq.filter (String.IsNullOrWhiteSpace >> not)
            |> Set.ofSeq
        | _ ->
            // Fallback: treat file as plain list of words, one per line
            File.ReadAllLines(path)
            |> Seq.map (fun line -> line.Trim())
            |> Seq.filter (String.IsNullOrWhiteSpace >> not)
            |> Set.ofSeq

    /// Filter a frequency map by removing entries present in the external dictionary set.
    /// Signature: existingSet -> freqMap -> filteredMap
    let filterByExternalDict (existing: Set<string>) (freqMap: Map<string,int>) : Map<string,int> =
        if Set.isEmpty existing then
            freqMap
        else
            freqMap |> Map.filter (fun word _ -> not (Set.contains word existing))
