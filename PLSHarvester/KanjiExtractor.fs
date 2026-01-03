namespace PLSHarvester

open System.Text.RegularExpressions

module KanjiExtractor =

    /// Regular expression that extracts consecutive Kanji sequences
    /// Kanji definition follows common Unicode CJK ranges + explicit chars ÅY and Éñ
    let private kanjiRegex =
        Regex(@"[\u4E00-\u9FFF\u3400-\u4DBF\uF900-\uFAFF\u3006\u30F6]+", RegexOptions.Compiled)

    /// Extract Kanji word candidates and return a frequency dictionary
    let extractKanjiCandidates (text: string) : Map<string,int> =
        kanjiRegex.Matches(text)
        |> Seq.cast<Match>
        |> Seq.map (fun m -> m.Value)
        |> Seq.fold (fun acc word ->
            acc |> 
            Map.change word 
                (function
                    | None -> Some 1
                    | Some v -> Some (v + 1))
            ) Map.empty
